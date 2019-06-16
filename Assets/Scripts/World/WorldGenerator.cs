using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator
{
    const int CHUNK_X = 16;
    const int CHUNK_Y = 256;
    const int CHUNK_Z = 16;

    private Seed seed;

    public TerrainGenerator(Seed seed)
    {
        this.seed = seed;
    }

    public void Generate(Chunk chunk)
    {
        for (int x = 0; x < CHUNK_X; x++) // local x
        {
            for (int z = 0; z < CHUNK_Z; z++) // local z
            {
                int ground = GetGroundAt(x, z);

                for (int y = 0; y < CHUNK_Y; y++) // local y
                {
                    if (ground < y)
                    {
                        chunk.SetBlock(x, y, z, 0); // Empty block
                    }
                    else
                    {
                        chunk.SetBlock(x, y, z, 1); // Stone block                      
                    }
                }
            }
        }
    }

    private int GetGroundAt(int x, int y)
    {
        seed.Reset();
        return Mathf.FloorToInt(Mathf.PerlinNoise(x + seed.get.Next(64), y + seed.get.Next(64)) * 125f);
    }
}
