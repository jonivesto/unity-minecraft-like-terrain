using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class GrassDecorator
{
    public static void GenerateGrass(TerrainGenerator t)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                int ground = t.GetGroundAt(x + t.worldX, z + t.worldZ);
                Biome biome = t.GetBiomeAt(x + t.worldX, z + t.worldZ, ground);

                if (biome.grass != 0 
                    && t.chunk.GetBlock(x, ground+1, z) == 0
                    && t.chunk.GetBlock(x, ground, z) == 9)
                {
                    t.chunk.SetBlock(x, ground + 1, z, biome.grass);
                }
            }
        }
    }

}