using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueCylinder : MonoBehaviour
{
    public GameObject arrow;
    public List<GameObject> dials;
    public List<GameObject> riddles;

    private int currentDial;
    private bool rotating;

    private int[] combination = { 0, 0, 0, 0 };

    public bool solved;

    // Start is called before the first frame update
    void Start()
    {
        currentDial = 0;
        rotating = false;
        solved = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || JoystickCodes.Left)
        {
            if (currentDial != 0)
            {
                Vector3 arrowPos = arrow.transform.localPosition;
                arrowPos.z -= 1.3f;
                arrow.transform.localPosition = arrowPos;
                currentDial--;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || JoystickCodes.Right)
        {
            if (currentDial != 3)
            {
                Vector3 arrowPos = arrow.transform.localPosition;
                arrowPos.z += 1.3f;
                arrow.transform.localPosition = arrowPos;
                currentDial++;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || JoystickCodes.Up)
        {
            if (!rotating)
            {
                StartCoroutine(RotateDial(dials[currentDial], true));
                combination[currentDial] = (combination[currentDial] + 1) % 10;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || JoystickCodes.Down)
        {
            if (!rotating)
            {
                StartCoroutine(RotateDial(dials[currentDial], false));
                combination[currentDial] = ((combination[currentDial] - 1) + 10) % 10;
            }
        }


        if (Input.GetButtonDown("Interact"))
        {
            if (CheckSolution())
            {
                solved = true;
                Debug.Log(CheckSolution());
            }
            else
            {
                GameController.Master.CylinderWrongAnswer();
            }
        }
    }

    IEnumerator RotateDial(GameObject dial, bool direction)
    {
        rotating = true;

        Vector3 rotation = dial.transform.rotation.eulerAngles;
        rotation.z = direction ? rotation.z + 36 : rotation.z - 36;

        while (GetAngleDifference(dial.transform.rotation.eulerAngles.z, rotation.z) > 1)
        {
            dial.transform.rotation = Quaternion.Lerp(dial.transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * 25f);
            yield return new WaitForEndOfFrame();
        }

        dial.transform.rotation = Quaternion.Euler(rotation);

        rotating = false;
        yield return null;
    }

    float GetAngleDifference(float x, float y)
    {
        return Mathf.Min(((x + (360 - y)) % 360), (((360 - x) + y)) % 360);
    }

    bool CheckSolution()
    {
        return (combination[0] == 1 && combination[1] == 9 && combination[2] == 1 && combination[3] == 9);
    }
}
