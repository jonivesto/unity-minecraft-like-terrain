
using System.Collections.Generic;
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    public Seed seed;
    public TerrainGenerator worldGenerator;

    const int RENDER_DISTANCE = 6;

    int renderDistance;
    float sleepDistance;

    public ChunkTransform[] loadedChunks;
    public Vector2Int playerChunk = new Vector2Int();

    private Vector3 playerAnchor = new Vector3();
    private int loadDimension;

    void Start()
    {
        seed = new Seed("1944887891122446");
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


        // Render chunks if not already rendered
        Transform parentOfChunks = GameObject.Find("/Environment/World").transform;
        List<GameObject> whitelist = new List<GameObject>();

        foreach (ChunkTransform chunkTransform in loadedChunks)
        {         
            if (GetRenderedChunk(chunkTransform) == null)
            {
                GameObject chunk = new GameObject(chunkTransform.ToString());

                chunk.transform.parent = parentOfChunks;
                chunk.transform.position = chunkTransform.GetBlockPosition();

                chunk.AddComponent<MeshFilter>();
                chunk.AddComponent<MeshRenderer>();

                Chunk c = chunk.AddComponent<Chunk>();
                c.SetChunkTransform(chunkTransform);
                worldGenerator.Generate(c);
                c.Render();
            }

            // Whitelist these chunks so they dont get destroyed
            whitelist.Add(GetRenderedChunk(chunkTransform));
        }


        // Destroy chunks that are not whitelisted
        for (int i = 0; i < parentOfChunks.childCount; ++i)
        {
            GameObject c = parentOfChunks.GetChild(i).gameObject;
            if (!whitelist.Contains(c))
            {
                Destroy(c);
            }
        }
    }

    private GameObject GetRenderedChunk(ChunkTransform chunkTransform)
    {
        return GameObject.Find("/Environment/World/" + chunkTransform.ToString());
    }
}

