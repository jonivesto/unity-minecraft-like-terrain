using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    WorldEngine worldEngine;

    void FixedUpdate()
    {
        // update world position
        worldEngine.UpdatePosition(transform.position);

    }

    void Start()
    {
        worldEngine = GameObject.Find("/Environment/World").GetComponent<WorldEngine>();
    }

}
