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
    private Vector3 playerAnchor = new Vector3();

    void Start()
    {
        seed = new Seed("1944887891122446");

        SetDistances(4);

    }

    void SetDistances(int renderDistance)
    {
        this.renderDistance = renderDistance;

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
        // Get chunk position player is in
        playerChunk.Set
        (
            Mathf.FloorToInt(position.x / 16),
            Mathf.FloorToInt(position.z / 16)
        );

        // Check if player has crossed the sleep distance      
        position.y = 0f;
        if (Vector3.Distance(position, playerAnchor) > (float)sleepDistance * 16f)
        {
            // Get new chunks to display   
            LoadPosition();

            // Update player anchor so the load will not happen again
            playerAnchor = position;
        }
    }

    private void LoadPosition()
    {
        
    }
}
