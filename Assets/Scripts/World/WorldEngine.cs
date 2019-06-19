﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class WorldEngine : MonoBehaviour
{
    public Seed seed;
    public TerrainGenerator terrainGenerator;
    public ChunkTransform[] loadedChunks;
    public Vector2Int playerChunk = new Vector2Int();

    int renderDistance;  
    int unloadDistance;
    int loadDimension;

    float sleepDistance;

    private Vector3 playerAnchor;
    private Vector3 facingDirection;
    private Coroutine load;

    void Start()
    {
        //seed = new Seed();
        seed = new Seed("0004887891122446");
        terrainGenerator = new TerrainGenerator(this);

        SetDistances(4);
        LoadPosition();
    }

    void SetDistances(int renderDistance)
    {
        this.renderDistance = renderDistance;

        if (renderDistance % 2 == 0)
        {
            renderDistance++;
        }

        sleepDistance = renderDistance / 2f;

        unloadDistance = renderDistance * 2;

        loadDimension = renderDistance * 2 + 1;
    }

    public void UpdatePosition(Vector3 position, Vector3 rotation)
    {
        // Get chunk position player is in
        playerChunk.Set
        (
            Mathf.FloorToInt(position.x / 16),
            Mathf.FloorToInt(position.z / 16)
        );

        // Check if player has crossed the sleep distance      
        position.y = 0f;
        if (Vector3.Distance(position, playerAnchor) > sleepDistance * 16f)
        {
            // Get new chunks to display   
            LoadPosition();

            // Update player anchor so the load will not happen again
            playerAnchor = position;
        }

        // Update Rotation
        facingDirection = rotation;
    }

    private void LoadPosition()
    {
        // Chunks around the player chunk in spiral order
        List<ChunkTransform> chunkTransforms = new List<ChunkTransform>();

        // Add player chunk (the center of the spiral)
        chunkTransforms.Add(new ChunkTransform(playerChunk));

        // X and Y directions that define the spiral pattern
        int[] patternX = new int[] {1, 0, 0, -1, -1, 0, 0, 1};
        int[] patternY = new int[] {0, -1, -1, 0, 0, 1, 1, 0};

        // Current position when drawing the spiral
        int x, y;
        
        for (int layer = 1; layer <= renderDistance; layer++) // Spiral layers aroud the player
        {
            // Set starting point for each layer
            x = playerChunk.x;
            y = playerChunk.y + layer;

            // Do each pattern for each layer (phase = index in patternX[] and patternY[])
            for (int phase = 0; phase < 8; phase++) 
            {
                // Split each pattern into steps (1 step = 1 chunk)
                for (int step = 1; step <= layer; step++) 
                {
                    // Set current position
                    x += patternX[phase] * 1;
                    y += patternY[phase] * 1;

                    chunkTransforms.Add(new ChunkTransform(x, y));
                }
            }
        }

        loadedChunks = chunkTransforms.ToArray();

        // Stop load if it is in progress
        if (load != null)
        {
            StopCoroutine(load);
        }

        // Start new load
        // true = load chunks smoothly in a spiral order
        // false = load all chunks at once. This causes lag.
        load = StartCoroutine("LoadChunks", true);
    }

    IEnumerator LoadChunks(bool async)
    {      
        Transform parentOfChunks = GameObject.Find("/Environment/World").transform;

        // Render chunks if not already rendered
        foreach (ChunkTransform chunkTransform in loadedChunks)
        {
            if (GetRenderedChunk(chunkTransform) == null)
            {
                GameObject obj = new GameObject(chunkTransform.ToString());

                obj.transform.parent = parentOfChunks;
                obj.transform.position = chunkTransform.GetBlockPosition();

                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>();
                obj.AddComponent<MeshCollider>();

                Chunk chunk = obj.AddComponent<Chunk>();
                chunk.chunkTransform = chunkTransform;

                terrainGenerator.Generate(chunk);
                chunk.Render();
            }
          
            // Destroy chunks that are too far away
            for (int i = 0; i < parentOfChunks.childCount; ++i)
            {
                Vector3 t = parentOfChunks.GetChild(i).position;
                if (Vector2Int.Distance(playerChunk * 16, new Vector2Int((int)t.x, (int)t.z)) > unloadDistance * 16)
                {
                    Destroy(parentOfChunks.GetChild(i).gameObject);
                }
            }
            
            // If true, load only one chunk and continue on next frame
            if (async)
            {
                yield return null;
            }
        }
    }

    // Returns chunk GameObject that is currently visible in the game
    // Return value can be null
    private GameObject GetRenderedChunk(ChunkTransform chunkTransform)
    {
        return GameObject.Find("/Environment/World/" + chunkTransform.ToString());
    }
}

