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

        //GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //cylinder.transform.position = transform.position;
        //cylinder.transform.parent = transform;

        if (false) // Check if this chunk is saved to a file
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
        
        // Plain sky and terrain
        for (int i = 0; i < 16; i++) // local x
        {
            for (int j = 0; j < 16; j++) // local z
            {
                int terrainNoise = Mathf.FloorToInt(Mathf.PerlinNoise(i + seed.get.Next(64), j + seed.get.Next(64)) * 125f);

                for (int y = 0; y < 256; y++) // local y
                {
                    if (terrainNoise < y)
                    {
                        // Empty block
                        blocks[i, y, j] = 0;
                    }
                    else
                    {
                        // Stone block
                        blocks[i, y, j] = 1;

                        // Bedrock
                        if (y == 0)
                        {                       
                            blocks[i, 0, j] = 2;

                            if (terrainNoise > 0.4)
                            {
                                blocks[i, 1, j] = 2;
                            }
                             
                        }
                    }
                }
            }
        }

    }

    
}
