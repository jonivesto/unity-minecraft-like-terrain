
using UnityEngine;

public class WorldEngine : MonoBehaviour
{
    public Seed seed;

    int renderDistance, sleepDistance;

    public Vector2Int[] loadedChunks;
    public Vector2Int playerChunk = new Vector2Int();

    private Vector3 playerAnchor = new Vector3();
    private int loadDimension;

    void Start()
    {
        seed = new Seed("1944887891122446");

        SetDistances();
        LoadPosition();

    }

    void SetDistances()
    {
        renderDistance = 15;

        if (renderDistance % 2 == 0)
        {
            renderDistance++;
        }

        sleepDistance = renderDistance / 3;
   
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

        loadedChunks = new Vector2Int[loadDimension * loadDimension];

        // Get chunks surrounding the player position
        int t = 0;
        for (int i = playerChunk.x - renderDistance; i < playerChunk.x + renderDistance; i++)
        {
            for (int j = playerChunk.y - renderDistance; j < playerChunk.y + renderDistance; j++)
            {
                loadedChunks[t] = new Vector2Int(i, j);
                t++;
            }
        }


        // Print for debug
        /*foreach(Vector2Int v2int in loadedChunks)
        {
            print(v2int.ToString());
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(v2int.x*16, 20f, v2int.y*16);
        }*/
    }

    private GameObject GetRenderedChunk(ChunkTransform chunkTransform)
    {
        return GameObject.Find("/Environment/World/" + chunkTransform.ToString());
    }
}

