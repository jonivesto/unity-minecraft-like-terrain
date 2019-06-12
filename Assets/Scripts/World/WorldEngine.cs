using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    public Seed seed;

    int renderDistance, loadDistance, sleepDistance;
    int anchorX, anchorZ;


    public Chunk[,,] loadedChunks;

    public Vector3 playerPosition;
    public Vector3Int playerChunk;


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
        playerPosition = position;
        playerChunk = GetChunkPosition(position);
    }

    private Vector3Int GetChunkPosition(Vector3 position)
    {
        return new Vector3Int
        (
            Mathf.FloorToInt(position.x / 16),
            Mathf.FloorToInt(position.y / 16),
            Mathf.FloorToInt(position.z / 16)
        );
    }

}
