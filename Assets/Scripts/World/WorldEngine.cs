using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    public int renderDistance = 3 * 16;

    public Seed worldSeed;

    const int WORLD_HEIGHT = 16 * 16;

    void Start()
    {  
        worldSeed = new Seed();
    }

    void UpdatePosition(float x, float y, float z)
    {
        
    }

    
}
