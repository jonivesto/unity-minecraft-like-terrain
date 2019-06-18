using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    public Seed seed;
    public TerrainGenerator worldGenerator;

    const int RENDER_DISTANCE = 6;

    int renderDistance;
    float sleepDistance;
    int unloadDistance;

    public ChunkTransform[] loadedChunks;
    public Vector2Int playerChunk = new Vector2Int();

    private Vector3 playerAnchor = new Vector3();
    private int loadDimension;
    private Coroutine load;

    void Start()
    {
        //seed = new Seed();
        seed = new Seed("0004887891122446");
        worldGenerator = new TerrainGenerator(this);

        SetDistances();
        LoadPosition();
    }

    void SetDistances()
    {
        renderDistance = RENDER_DISTANCE;

        if (renderDistance % 2 == 0)
        {
            renderDistance++;
        }

        sleepDistance = renderDistance / 2.5f;

        unloadDistance = renderDistance * 2;

        loadDimension = renderDistance * 2 + 1;
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
        if (Vector3.Distance(position, playerAnchor) > sleepDistance * 16f)
        {
            // Get new chunks to display   
            LoadPosition();

            // Update player anchor so the load will not happen again
            playerAnchor = position;
        }
    }

    private void LoadPosition()
    {
        loadedChunks = new ChunkTransform[loadDimension * loadDimension];

        // Get chunks surrounding the player position
        int t = 0;
        for (int i = playerChunk.x - renderDistance; i <= playerChunk.x + renderDistance; i++)
        {
            for (int j = playerChunk.y - renderDistance; j <= playerChunk.y + renderDistance; j++)
            {
                loadedChunks[t] = new ChunkTransform(i, j);
                t++;
            }
        }

        // Stop load if it is in progress
        if(load != null)
        {
            StopCoroutine(load);
        }
        
        // Choose load performance type
        // If current Player chunk is not rendered, use hard load      
        load = StartCoroutine("LoadChunks", GetRenderedChunk(new ChunkTransform(playerChunk)) != null);
        
    }

    IEnumerator LoadChunks(bool async)
    {
        // Render chunks if not already rendered
        Transform parentOfChunks = GameObject.Find("/Environment/World").transform;

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

                worldGenerator.Generate(chunk);
                chunk.Render();
            }


            // Destroy chunks that are not in range
            for (int i = 0; i < parentOfChunks.childCount; ++i)
            {
                Vector3 t = parentOfChunks.GetChild(i).position;
                if (Vector2Int.Distance(playerChunk, new Vector2Int((int)t.x, (int)t.z)) > unloadDistance * 16)
                {
                    Destroy(parentOfChunks.GetChild(i).gameObject);
                }
            }

            if (async)
            {
                yield return null;
            }
        }

    }

    private GameObject GetRenderedChunk(ChunkTransform chunkTransform)
    {
        return GameObject.Find("/Environment/World/" + chunkTransform.ToString());
    }
}

