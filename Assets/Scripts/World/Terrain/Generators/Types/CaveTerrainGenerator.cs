using NoiseTest;
using UnityEngine;
using System;
using System.Collections.Generic;

public class CaveTerrainGenerator : TerrainGenerator
{
    public CaveTerrainGenerator(TerrainEngine terrainEngine) : base(terrainEngine) { }

    public override void Generate(Chunk chunk)
    {
        SetChunk(chunk);

        for (int x = 0; x < CHUNK_SIZE; x++) // local x
        {
            for (int z = 0; z < CHUNK_SIZE; z++) // local z
            {
                for (int y = 0; y < Config.WORLD_HEIGHT; y++) // local y
                {
                    if (y == 0 || y == Config.WORLD_HEIGHT-1)
                    {
                        chunk.SetBlock(x, y, z, 2); // Bedrock
                    }

                    else if(simplex1.Evaluate((x + worldX) / 16f, y / 16f, (z + worldZ) / 16f) < 0.5)
                    {
                        chunk.SetBlock(x, y, z, 1); // Stone
                    }
                    
                }
            }
        }

    }
}
