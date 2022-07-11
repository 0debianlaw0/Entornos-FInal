using System;
using UnityEngine;

public class InputData : MonoBehaviour
{
    //Movimiento basico
    public float hMovement;
    public float vMovement;

    //Rotacion del raton
    public float verticalMouse;
    public float horizontalMouse;

    //Movimiento extra
    public bool dash;
    public bool jump;

    public void getInput()
    {
        //Movimiento basico
        hMovement = Input.GetAxis("Horizontal");
        vMovement = Input.GetAxis("Vertical");

        //Rotacion Raton/Joystick
        verticalMouse = Input.GetAxis("Mouse Y");
        horizontalMouse = Input.GetAxis("Mouse X");

        //Movimiento extra
        dash = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetButton("Jump");
    }
}
