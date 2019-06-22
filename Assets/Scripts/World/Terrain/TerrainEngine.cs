﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TerrainEngine : MonoBehaviour
{
    public string worldName;
    public Seed seed;
    public Config config = new Config();
    public Vector2Int playerChunk = new Vector2Int();
    
    TerrainGenerator terrainGenerator;
    ChunkRenderer chunkRenderer = new ChunkRenderer();
    ChunkTransform[] loadedChunks, renderedChunks;
    Vector3 playerAnchor;
    Vector3 facingDirection;
    Coroutine load;
    Save save;

    int renderDistance;
    int preLoadDistance;
    int unloadDistance;
    int loadDimension;

    float sleepDistance;


    void Start()
    {
        worldName = "My world";
        seed = new Seed("0004887891122446");

        save = new Save(worldName, seed);
        terrainGenerator = new TerrainGenerator(this);

        SetDistances(2, 1);
        LoadPosition();
    }

    // Set all distances to the according to the renderdistance
    // Call this on init
    // call this on render distance setting changes
    void SetDistances(int renderDistance, int preLoadDistance)
    {
        this.renderDistance = renderDistance;

        // Make sure renderDistance is odd
        if (renderDistance % 2 == 0)
        {
            renderDistance++;
        }

        this.preLoadDistance = renderDistance + preLoadDistance;

        // Distance required between player and anchor to start new chunk to load
        sleepDistance = renderDistance / 2f;

        // Distance between player and chunks
        // When out of range, chunks will be unloaded
        unloadDistance = renderDistance * 2;

        // x and y lenghts of the loadedChunks[] array
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
        // LoadedChunks will always have more layer(s) than renderedChunks
        List<ChunkTransform> loadedChunkTransforms = new List<ChunkTransform>();
        List<ChunkTransform> renderedChunkTransforms = new List<ChunkTransform>();

        // Add player chunk (the center of the spiral)
        loadedChunkTransforms.Add(new ChunkTransform(playerChunk));
        renderedChunkTransforms.Add(new ChunkTransform(playerChunk));

        // X and Y directions that define the spiral pattern
        int[] patternX = new int[] {1, 0, 0, -1, -1, 0, 0, 1};
        int[] patternY = new int[] {0, -1, -1, 0, 0, 1, 1, 0};

        // Current position when drawing the spiral
        int x, y;
        
        for (int layer = 1; layer <= renderDistance + preLoadDistance; layer++) // Spiral layers aroud the player
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

                    // Add to load
                    loadedChunkTransforms.Add(new ChunkTransform(x, y));

                    // Add to render
                    // RenderedChunks will have less layers
                    if(layer <= renderDistance)
                    {
                        renderedChunkTransforms.Add(new ChunkTransform(x, y));
                    }
                }
            }
        }

        // List to array
        loadedChunks = loadedChunkTransforms.ToArray();
        renderedChunks = renderedChunkTransforms.ToArray();

        // Stop load if it is in progress
        if (load != null)
        {
            StopCoroutine(load);
        }

        // Start new load      
        load = StartCoroutine("LoadChunks", true);
    }

    // Load all chunks defined in loadedChunks[] array
    // true = load chunks smoothly in a spiral order
    // false = load all chunks at once. This causes high lag.
    IEnumerator LoadChunks(bool async)
    {
        Debug.Log("Chunk loading started");
        Transform parentOfChunks = GameObject.Find("/Environment/World").transform;

        foreach (ChunkTransform chunkTransform in loadedChunks)
        {
            Chunk chunk;

            // Create gameObject if not exists
            if (GetChunk(chunkTransform) == null)
            {
                GameObject obj = new GameObject(chunkTransform.ToString());

                obj.transform.parent = parentOfChunks;
                obj.transform.position = chunkTransform.GetBlockPosition();

                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>();
                obj.AddComponent<MeshCollider>();

                chunk = obj.AddComponent<Chunk>();
                chunk.SetTransform(chunkTransform);        
            }
            else
            {
                chunk = GetChunk(chunkTransform);
            }

            // Generate / load from file
            if (!chunk.generated)
            {
                // Check if file exists
                if (save.ChunkFileExists(chunkTransform))
                {
                    // Load chunk from file
                    chunk.chunkData = save.LoadChunk(chunkTransform);
                }
                else
                {
                    // Generate and save to file
                    terrainGenerator.Generate(chunk);
                    if (Config.SAVE_CHUNKS_ON_GENERATE)
                    {
                        save.SaveChunk(chunk);
                    }
                    
                }

                chunk.generated = true;
            }


            // Destroy chunks that are too far away
            for (int i = 0; i < parentOfChunks.childCount; ++i)
            {
                // Chunk's world position
                Vector3 t = parentOfChunks.GetChild(i).position;

                if (Vector2Int.Distance(playerChunk * 16, new Vector2Int((int)t.x, (int)t.z)) > unloadDistance * 16)
                {
                    Chunk unload = parentOfChunks.GetChild(i).gameObject.GetComponent<Chunk>();

                    // Save if there are unsaved changes
                    if (chunk.unsaved)
                    {
                        save.SaveChunk(unload);
                    }

                    // Unload
                    Destroy(unload.gameObject);
                }
            }

            // If true, load one chunk and continue at next frame
            if (async)
            {
                yield return null;
            }
        }


        // Init chunks
        foreach (ChunkTransform chunkTransform in renderedChunks)
        {
            Chunk chunk = GetChunk(chunkTransform);

            if (chunk == null)
            {
                Debug.LogWarning("NULL");
                continue;
            }

            chunk.SetNext(
                GetChunk(chunkTransform.GetRight()),
                GetChunk(chunkTransform.GetLeft()),
                GetChunk(chunkTransform.GetFront()),
                GetChunk(chunkTransform.GetBack())
            );
        }

        Debug.Log("Chunks generated, rendering..");

        // Render chunks
        for (int i = 0; i < renderedChunks.Length; i++)
        {
            if(GetChunk(renderedChunks[i]) != null)
            {
                Chunk chunk = GetChunk(renderedChunks[i]);

                if (!chunk.rendered && chunk.generated)
                {
                    chunkRenderer.Render(chunk);
                    chunk.rendered = true;
                }      
            }
                
            // If true, render only one chunk and continue on next frame
            if (async)
            {
                yield return null;
            }
        }

        Debug.Log("Chunk loading finished");
    }

    // Save all loaded chunks to files
    // Call this before quit the world
    private void SaveLoadedChunks()
    {
        Transform parentOfChunks = GameObject.Find("/Environment/World").transform;

        for (int i = 0; i < parentOfChunks.childCount; ++i)
        {        
            Chunk chunk = parentOfChunks.GetChild(i).gameObject.GetComponent<Chunk>();
            save.SaveChunk(chunk);          
        }
    }

    // Returns chunk that is currently in the game hierarchy
    // Returns null if the chunk is not loaded
    private Chunk GetChunk(ChunkTransform chunkTransform)
    {
        GameObject obj = GameObject.Find("/Environment/World/" + chunkTransform.ToString());

        if (obj == null) return null;

        return obj.GetComponent<Chunk>();
    }
}

