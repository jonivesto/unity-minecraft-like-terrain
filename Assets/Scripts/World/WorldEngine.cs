using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    const int UNIT = 16;
    const int WORLD_HEIGHT = UNIT * UNIT;

    public int renderDistance, loadDistance;

    public Seed seed;

    public Chunk[,,] loadedChunks;
    public Chunk playerChunk;

    
    void Start()
    {
        seed = new Seed("1234567891123456");

        SetDistance();

        loadedChunks = new Chunk[loadDistance * 2 * UNIT, UNIT, loadDistance * 2 * UNIT];

        print(this.seed.get.Next(14));
        print(this.seed.get.Next(63));
        print(this.seed.get.Next(45));
        print(this.seed.get.Next(66));
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
        return null;
    }

}
