using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    WorldEngine worldEngine;
    Rigidbody rigidBody;

    private Transform playerCamera;
    private Vector3 movement;
    private float lastRotation;
    private float playerRotationX;
    private float playerRotationY;
    private float mouseLookSensitivity = 35f;

    void Start()
    {
        worldEngine = GameObject.Find("/Environment/World").GetComponent<WorldEngine>();
        rigidBody = GetComponent<Rigidbody>();
        playerCamera = transform.Find("Player Camera");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        // Player rotation
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0f, playerRotationY, 0f) * mouseLookSensitivity * Time.deltaTime);
        rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);

        // Player camera rotation               
        float clampAngle = 90f;
        lastRotation += playerRotationX * mouseLookSensitivity * Time.deltaTime;
        lastRotation = Mathf.Clamp(lastRotation, -clampAngle, clampAngle);
        playerCamera.localRotation = Quaternion.Euler(lastRotation, 0f, 0.0f);

        // Player movement
        rigidBody.AddForce(transform.TransformDirection(movement) * 13f * 5f);

        // Load world around the new position
        worldEngine.UpdatePosition(transform.position);
    }


    internal void Move(float x, float z)
    {
        movement = Vector3.ClampMagnitude(new Vector3(z, 0f, x), 0.8f);
    }

    internal void Jump()
    {
        rigidBody.AddForce(transform.up * 33f, ForceMode.Impulse);
    }

    internal void MouseLook(float x, float y)
    {
        playerRotationY = x;
        playerRotationX = y;
    }
}
