using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    const int WORLD_HEIGHT = 16 * 16;

    public int renderDistance, loadDistance;


    public Seed seed;

    public List<Chunk> loadedChunks = new List<Chunk>();
    public Chunk playerChunk;

    
    void Start()
    {
        //seed = new Seed("1234567891123456");
        seed = new Seed();

        SetDistance();

        

    }

    private void SetDistance()
    {
        renderDistance = 3;

        if (renderDistance % 2 == 0)
        {
            renderDistance++;
        }

        loadDistance = renderDistance + 2;
    }

    void UpdatePosition(Vector3 position)
    {
        playerChunk = GetChunkAt(position);
    }

    private Chunk GetChunkAt(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);
        

        return null;
    }

}
