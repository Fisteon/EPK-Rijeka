using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockCrazySpinning : MonoBehaviour
{
    public GameObject clockHandMinutes;
    public GameObject clockHandHours;

    public Vector3 rotationAxis = new Vector3(0, 1, 0);

    int factor;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ClockHandsSpin());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ClockHandsSpin()
    {
        factor = GameController.Master.randomNumberGenerator.Next(1, 5);
        Quaternion m = Quaternion.Euler(clockHandMinutes.transform.localRotation.eulerAngles);
        Quaternion h = Quaternion.Euler(clockHandHours.transform.localRotation.eulerAngles);
        while (true)
        {
            h *= Quaternion.Euler(new Vector3(rotationAxis.x * 1f, rotationAxis.y * 1f, rotationAxis.z * 1f) * factor);
            clockHandHours.transform.localRotation = h;

            m *= Quaternion.Euler(new Vector3(rotationAxis.x * 1f, rotationAxis.y * 1f, rotationAxis.z * 1f) * (factor * 12));
            clockHandMinutes.transform.localRotation = m;
            yield return new WaitForSeconds(0.025f);
        }
    }
}
