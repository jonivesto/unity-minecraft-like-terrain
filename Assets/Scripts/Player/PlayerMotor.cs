using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    WorldEngine worldEngine;

    void FixedUpdate()
    {

    }

    void Start()
    {
        worldEngine = GameObject.Find("/Environment/World").GetComponent<WorldEngine>();
    }

    internal static void Move(float x, float y)
    {
        throw new NotImplementedException();
    }

}
