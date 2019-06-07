using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    const int CHUNK_DIMENSION = 16;
    const int WORLD_HEIGHT = CHUNK_DIMENSION * 16;

    public int renderDistance = 3;

    public Chunk[,,] chunks;

    public Seed seed;

    
    void Start()
    {  
        seed = new Seed();
    
        chunks = new Chunk[renderDistance * 2 * 16, 16, renderDistance * 2 * 16];
    }

    void UpdatePosition(float x, float y, float z)
    {
        GetChunkAt(x, y, z);
    }

    private void GetChunkAt(float x, float y, float z)
    {
        int ix = Mathf.FloorToInt(x);
        int iy = Mathf.FloorToInt(y);
        int iz = Mathf.FloorToInt(z);

        Debug.Log("X: " + x/16 + "Y: " + y/16 + "Z: " + z/16);
    }


}
