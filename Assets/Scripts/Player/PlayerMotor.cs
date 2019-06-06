using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    WorldEngine worldEngine;

    // Start is called before the first frame update
    void Start()
    {
        worldEngine = GameObject.Find("/Environment/World").GetComponent<WorldEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
