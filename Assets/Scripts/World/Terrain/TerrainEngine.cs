using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class TerrainEngine : MonoBehaviour
{
    public string worldName;
    public Seed seed;
    public Config config = new Config();
    public Vector2Int playerChunk = new Vector2Int();
    
    TerrainGenerator terrainGenerator;
    ChunkRenderer chunkRenderer = new ChunkRenderer();
    ChunkTransform[] loadedChunks;
    Vector3 playerAnchor;
    Vector3 facingDirection;
    Coroutine load;
    Save save;

    int renderDistance;  
    int unloadDistance;
    int loadDimension;

    float sleepDistance;


    void Start()
    {
        worldName = "My world";
        //seed = new Seed();
        seed = new Seed("0004887891122446");

        save = new Save(worldName, seed);
        terrainGenerator = new TerrainGenerator(this);

        SetDistances(4);
        LoadPosition();
    }

    // Set all distances to the according to the renderdistance
    // Call this on init
    // call this on render distance setting changes
    void SetDistances(int renderDistance)
    {
        this.renderDistance = renderDistance;

        // Make sure renderDistance is odd
        if (renderDistance % 2 == 0)
        {
            renderDistance++;
        }

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

        // List to array
        loadedChunks = chunkTransforms.ToArray();

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
        Transform parentOfChunks = GameObject.Find("/Environment/World").transform;

        // Load chunks if not already in game
        foreach (ChunkTransform chunkTransform in loadedChunks)
        {
            if (GetChunk(chunkTransform) == null)
            {
                GameObject obj = new GameObject(chunkTransform.ToString());

                obj.transform.parent = parentOfChunks;
                obj.transform.position = chunkTransform.GetBlockPosition();

                obj.AddComponent<MeshFilter>();
                obj.AddComponent<MeshRenderer>();
                obj.AddComponent<MeshCollider>();

                Chunk chunk = obj.AddComponent<Chunk>();
                chunk.Init(chunkTransform);

                // Check if file exists
                if (save.ChunkFileExists(chunkTransform))
                {
                    // Load chunk from file
                    chunk.chunkData = save.LoadChunk(chunkTransform);
                }
                else
                {
                    // Generate chunk
                    terrainGenerator.Generate(chunk);                   
                }
                
                // Render chunk
                chunkRenderer.Render(chunk);           
            }
          
            // Destroy chunks that are too far away
            // Before that, save them to file
            for (int i = 0; i < parentOfChunks.childCount; ++i)
            {
                Vector3 t = parentOfChunks.GetChild(i).position;
                if (Vector2Int.Distance(playerChunk * 16, new Vector2Int((int)t.x, (int)t.z)) > unloadDistance * 16)
                {
                    GameObject chunkObj = parentOfChunks.GetChild(i).gameObject;
                    save.SaveChunk(chunkObj.GetComponent<Chunk>());
                    Destroy(chunkObj);
                }
            }
            
            // If true, load only one chunk and continue on next frame
            if (async)
            {
                yield return null;
            }
        }
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

    // Returns chunk GameObject that is currently in the game hierarchy
    // Returns null if the chunk is not loaded
    private GameObject GetChunk(ChunkTransform chunkTransform)
    {
        return GameObject.Find("/Environment/World/" + chunkTransform.ToString());
    }
}

