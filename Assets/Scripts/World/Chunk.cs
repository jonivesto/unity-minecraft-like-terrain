using System;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    WorldEngine worldEngine;
    ChunkTransform chunkTransform;

    private int x, z;

    int[,,] blocks = new int[16, 256, 16];

    internal void Construct(WorldEngine worldEngine, ChunkTransform chunkTransform)
    {
        this.worldEngine = worldEngine;
        this.chunkTransform = chunkTransform;

        x = chunkTransform.x;
        z = chunkTransform.z;



        // Debug
        //GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //cylinder.transform.position = transform.position;
        //cylinder.transform.parent = transform;
    }

    public void Load()
    {
        if (false) // Todo
        {
            // Load from file
        }
        else
        {
            Generate();
        }
    }

    void Generate()
    {
        Seed seed = worldEngine.seed;
        seed.Reset();
        /*
        // Plain sky and terrain
        for (float i = x; i < x + 16; i++)
        {
            for (float j = z; j < z + 16; j++)
            {
                int terrainNoise = Mathf.FloorToInt(Mathf.PerlinNoise(x + seed.get.Next(64), y + seed.get.Next(64)) * 125f);

                for (float t = y; t < y + 16; t++)
                {
                    if (terrainNoise > t)
                    {
                        // empty block
                        blocks[x, y, z] = 0;
                    }
                    else
                    {
                        // stone block
                        blocks[x, y, z] = 1;
                    }
                }
            }
        }

        // Bedrock
        if (y == 0) {

            for (float i = x; i < x + 16; i++)
            {
                for (float j = z; j < z + 16; j++)
                {
                    blocks[x, 0, z] = 2;

                    if (Mathf.PerlinNoise(x + seed.get.Next(64), y + seed.get.Next(64)) > 0.4)
                    {
                        blocks[x, 1, z] = 2;
                    }
                }
            }

        }
        */
    }

    
}
