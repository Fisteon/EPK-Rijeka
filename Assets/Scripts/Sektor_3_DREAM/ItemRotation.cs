using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotation : MonoBehaviour
{
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.name == "FinalToiletPaper")
        {
            this.transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed, Space.Self);
        }
        else
        {
            this.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.Self);
        }
    }
}
