using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    WorldEngine worldEngine;

    void FixedUpdate()
    {
        // move player
        transform.Translate(Vector3.left*Time.deltaTime*0.9f);
        // update world position
        worldEngine.UpdatePosition(transform.position);

    }

    void Start()
    {
        worldEngine = GameObject.Find("/Environment/World").GetComponent<WorldEngine>();
    }

}
