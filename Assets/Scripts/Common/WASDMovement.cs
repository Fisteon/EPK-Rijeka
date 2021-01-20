using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WASDMovement : MonoBehaviour
{
    public List<GameObject> testing;
    public Dictionary<string, GameObject> directions;
    RaycastHit hitInfo;

    public static bool rotationLeft;
    public static bool rotationRight;
    public static bool walkingForward;
    public static bool walkingBack;
    public static bool turnAround;
    public static bool deadzoning;
    public static bool deadzoningINSIDE;
    public static bool deadzoningOUTSIDE;

    // Start is called before the first frame update
    void Start()
    {
        directions = new Dictionary<string, GameObject>();
        foreach(GameObject o in testing)
        {
            directions.Add(o.transform.name.Split('-')[1], o);
        }
        deadzoning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (deadzoning)
        {
            if (deadzoningINSIDE)
            {
                transform.localPosition = transform.localPosition;
            }
            else if (deadzoningOUTSIDE)
            {
                walkingBack = false;
                transform.localPosition = directions["W"].transform.localPosition;
            }
            setRotation(false);
            return;
        }

        if (GameController.Master.cutscene || GameController.Master.questSolving)
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
            setRotation(false);
            return;
        }
        #region Keyboard Movement
        /*
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            setRotation(false);
            walkingForward = true;
            rotationRight = true;
            transform.localPosition = directions["WD"].transform.localPosition;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            walkingForward = true;
            rotationLeft = true; 
            transform.localPosition = directions["WA"].transform.localPosition;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            walkingForward = true;
            transform.localPosition = directions["W"].transform.localPosition;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            //transform.localPosition = directions["SA"].transform.localPosition;
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            //transform.localPosition = directions["SD"].transform.localPosition;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            walkingBack = true;
            transform.localPosition = directions["S"].transform.localPosition;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rotationLeft = true;
            transform.localPosition = directions["A"].transform.localPosition;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationRight = true;
            transform.localPosition = directions["D"].transform.localPosition;
        }
        else
        {
            transform.localPosition = new Vector3(0, 1, 0.25f);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            rotationLeft = false;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            rotationRight = false;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            walkingBack = false;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            transform.localPosition = new Vector3(0, 1, 0.25f);
            walkingBack = false;
            walkingForward = false;
            setRotation(false);
        }
        */
        #endregion

        #region Joystick Movement
        float destinationX = Input.GetAxis("JoystickVertical");
        float destinationY = Input.GetAxis("JoystickHorizontal");

        if (destinationX < 0)
        {
            walkingBack = true;
            destinationY = 0;
        }
        else
        {
            walkingBack = false;
        }

        transform.localPosition = new Vector3(destinationY, transform.localPosition.y, destinationX);

        #endregion


    }

    private void setRotation(bool r)
     {
        rotationLeft = r;
        rotationRight = r;
        turnAround = r;
    }
}
