using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour {

    public Transform target;
    public Camera flyingCamera;

    public float xSpeed;
    public float ySpeed;

    public int yMinLimit;
    public int yMaxLimit;

    private float x = -24.0f;
    private float y = 0f;

    public float initialDistance = 4.234825f;

    Vector3 initialPosition;


    void Start()
    {
        initialPosition = flyingCamera.transform.localPosition;
    }

    private void Update()
    {
        RaycastHit hitInfo;
        Vector3 rayStart = this.transform.position;
        if (Physics.Raycast(rayStart, -(transform.position - flyingCamera.transform.position), out hitInfo, initialDistance))
        {

            //Debug.Log("Test - " + hitInfo.transform.name);
            if (hitInfo.collider.tag == "impassable")
            {
                flyingCamera.transform.position = hitInfo.point;
            }
        }
        else
        {
            //Debug.Log("Test - no collider");
            if (Vector3.Distance(transform.position, flyingCamera.transform.position) < initialDistance)
            {
                Vector3 direction;
                direction = -(transform.position - flyingCamera.transform.position).normalized * initialDistance;
                Debug.DrawRay(transform.position, direction);
                flyingCamera.transform.position = transform.position + direction;
            }
        }

        Vector3 d;
        d = -(transform.position - flyingCamera.transform.position).normalized * initialDistance;
        Debug.DrawRay(transform.position, d);
    }

    /**
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */
    void LateUpdate()
    {
        // Don't do anything if target is not defined
        if (!target)
            return;

        if (!flyingCamera.enabled || WASDMovement.deadzoning)
        {
            y = 0;
            return;
        }

        // If either mouse buttons are down, let the mouse govern camera position
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            x -= Input.GetAxis("Mouse Y") * ySpeed;
            y = Input.GetAxis("Mouse X") * xSpeed;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            y = 0;
            return;
        }

        if (true /*joystick connected*/)
        {
            x += Input.GetAxis("JoystickCameraVertical");
            y = Input.GetAxis("JoystickCameraHorizontal") * ySpeed;
        } 

        x = ClampAngle(x, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);

        PlayerController._PlayerController.transform.Rotate(new Vector3(0, y, 0));
        transform.localRotation = rotation;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

}
