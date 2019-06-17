using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMotor playerMotor;

    void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
    }

    // Get input for each frame
    void Update()
    {
        // Get WASD
        float x = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Horizontal");
        playerMotor.Move(x, z);

        // Get mouse
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = -Input.GetAxisRaw("Mouse Y");
        playerMotor.MouseLook(mouseX, mouseY);

        // Get right mouse button
        if (Input.GetButtonDown("Fire2")) { }
        if (Input.GetButtonUp("Fire2")) { }

        // Get Left mouse button
        if (Input.GetButtonDown("Fire1")) { }
        if (Input.GetButtonUp("Fire1")) { }

        // Get spacebar
        if (Input.GetButtonDown("Jump")) { playerMotor.Jump(); }

        // Get mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
    }
}
