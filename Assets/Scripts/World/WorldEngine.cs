using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    public Seed seed;

    int renderDistance, loadDistance, sleepDistance;


    public Chunk[,] loadedChunks;

    public Vector2Int playerChunk = new Vector2Int();


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

        loadedChunks = new Chunk[loadDistance * 2 + 1, loadDistance * 2 + 1];
    }

    public void UpdatePosition(Vector3 position)
    {
        playerChunk.Set(
            Mathf.FloorToInt(position.x / 16),
            Mathf.FloorToInt(position.z / 16));


    }

    

}
