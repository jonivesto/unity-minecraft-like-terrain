using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    public Seed seed;

    public int renderDistance, loadDistance, sleepDistance;

    public Chunk[,,] loadedChunks;
    public Chunk playerChunk;


    void Start()
    {
        seed = new Seed("1944887891122446");

        SetDistance();

    }

    void SetDistance()
    {

        renderDistance = 3;

        if (renderDistance % 2 == 0)
        {
            renderDistance++;
        }

        sleepDistance = renderDistance - 1;

        loadDistance = renderDistance + 2;

        loadedChunks = new Chunk[loadDistance * 2 + 1, 16 * 16, loadDistance * 2 + 1];
    }

    public void UpdatePosition(Vector3 position)
    {
        //playerChunk = GetChunkAt(position);

        
    }

    /*private Chunk GetChunkAt(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / 16);
        int y = Mathf.FloorToInt(position.y / 16);
        int z = Mathf.FloorToInt(position.z / 16);
        
        Chunk chunk = loadedChunks[x][y][z]

        return new Chunk(this, x, y, z);
    }*/

}
