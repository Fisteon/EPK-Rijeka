using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickCodes : MonoBehaviour
{
    public static bool Left, Right, Up, Down;
    float _LastX, _LastY;

    private void Update()
    {
        float x = Input.GetAxis("JoystickLeftRight");
        float y = Input.GetAxis("JoystickUpDown");

        Left = Right = Up = Down = false;

        if (_LastX != x)
        {
            if (x == -1) Left = true;
            else if (x == 1) Right = true;
        }

        if (_LastY != y)
        {
            if (y == -1) Down = true;
            else if (y == 1) Up = true;
        }

        _LastX = x;
        _LastY = y;
    }
}