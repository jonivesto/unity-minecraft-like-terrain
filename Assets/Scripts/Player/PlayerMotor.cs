using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    TerrainEngine terrainEngine;
    Rigidbody rigidBody;

    private Transform playerCamera;
    private Vector3 movement;
    private float lastRotation;
    private float playerRotationX;
    private float playerRotationY;
    private float mouseLookSensitivity = 35f;
    private Vector3Int selectedBlock;

    void Start()
    {
        terrainEngine = GameObject.Find("/Environment/World").GetComponent<TerrainEngine>();
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
        ShootRaycast();

        // Player movement
        rigidBody.AddForce(transform.TransformDirection(movement) * 13f * 5f);

        // Load world around the new position
        terrainEngine.UpdatePosition(transform.position, transform.rotation.eulerAngles);
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

    internal void PrimaryFire()
    {
        int x = selectedBlock.x;
        int y = selectedBlock.y;
        int z = selectedBlock.z;
        terrainEngine.WorldSetBlockRefresh(x, y, z, 0);
    }

    internal void SecondaryFire()
    {
        RaycastHit hit;

        // Setting block requires + normal, not -
        if (Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward) * 1.1f, out hit, Mathf.Infinity))
        {
            selectedBlock = new Vector3Int(
                Mathf.FloorToInt(hit.point.x + (hit.normal.x) / 2),
                Mathf.FloorToInt(hit.point.y + (hit.normal.y) / 2),
                Mathf.FloorToInt(hit.point.z + (hit.normal.z) / 2));

        }

        int x = selectedBlock.x;
        int y = selectedBlock.y;
        int z = selectedBlock.z;
        terrainEngine.WorldSetBlockRefresh(x, y, z, 5);
    }

    internal void ShootRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward) * 1.1f, out hit, Mathf.Infinity))
        {
            selectedBlock = new Vector3Int(
                Mathf.FloorToInt(hit.point.x - (hit.normal.x) / 2), 
                Mathf.FloorToInt(hit.point.y - (hit.normal.y) / 2), 
                Mathf.FloorToInt(hit.point.z - (hit.normal.z) / 2));
        }
    }
}
