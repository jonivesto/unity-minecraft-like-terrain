using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Get input for each frame
    void Update()
    {
        PlayerMotor.Move(Input.GetAxis("horizontal"), Input.GetAxis("vertical"));
    }
}
