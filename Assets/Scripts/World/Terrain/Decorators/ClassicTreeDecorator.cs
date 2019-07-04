using System;
using UnityEngine;

static class ClassicTreeDecorator
{
    public static void GenerateTrees(TerrainGenerator t)
    {
        // Pick a random starting point
        System.Random r = t.seed.ChunkBuild(t.chunk.chunkTransform);

        // Tree position
        int x = r.Next(16);      
        int z = r.Next(16);
        int y = t.GetGroundAt(x + t.worldX, z + t.worldZ);

        // Biome the tree is on
        Biome biome = t.GetBiomeAt(x, z, y);

        // Dont make trees on flat
        if (t.GetHillsAt(x + t.worldX, z + t.worldZ) == 0) return;

        // If picked point biome can grow trees
        for (int id = 0; id < biome.treesPerChunk; id++)
        {
            // New position for each tree
            x = r.Next(16);          
            z = r.Next(16);
            y = t.GetGroundAt(x + t.worldX, z + t.worldZ);

            biome = t.GetBiomeAt(x, z, y);

            // Block ID the tree will be created on
            int block = t.chunk.GetBlock(x, y, z);

            // Only grass and dirt can grow trees (IDs: 9, 8)
            if (block != 9 && block != 8) continue;

            // Height for tree based on biome's tree sizes
            int treeHeight = r.Next(biome.minTreeHeight, biome.maxTreeHeight);

            // Skip this tree if the terrain does not allow tree in this position
            if (TreeBlocked(x + t.worldX, y, z + t.worldZ, treeHeight, t.terrainEngine)) continue;
            
            // Create tree
            // Set wood blocks
            for (int i = 1; i < treeHeight; i++)
            {
                t.chunk.SetBlock(x, y + i, z, biome.woodBlock);
            }
            
            // Set leaves
            //TODO: Multiple leaf models
            t.chunk.SetBlock(x, y + treeHeight, z, biome.leavesBlock);

            int[] leavesX = new int[] { -1, -1, 1, 1, 0, 0, -1, 1, -1, -1, 1, 1, 0, 0, -1, 1, -1, -1, 1, 1, 0, 0, -1, 1 };
            int[] leavesY = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2, -2, -2, -2, -2, -2, };
            int[] leavesZ = new int[] { -1, 1, -1, 1, 1, -1, 0, 0, -1, 1, -1, 1, 1, -1, 0, 0, -1, 1, -1, 1, 1, -1, 0, 0 };
            for (int i = 0; i < leavesX.Length; i++)
            {
                t.terrainEngine.WorldSetBlock(x + t.worldX + leavesX[i], y + treeHeight + leavesY[i], z + t.worldZ + leavesZ[i], biome.leavesBlock);
            }
        }

    }

    private static bool TreeBlocked(int x, int y, int z, int h, TerrainEngine t)
    {
        // Surrounding blocks
        int[] patternX = new int[] { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] patternZ = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };

        for (int i = 1; i < h; i++)
        {          
            // If wood is blocked
            if (t.WorldGetBlock(x, y + i, z) != 0) return true;          

            // Don't check surroundings for the first wood block
            if (i == 1) continue;

            // If surrounding blocks are blocked
            for (int j = 0; j < 8; j++)
            {
                if (t.WorldGetBlock(x + patternX[j], y + i, z + patternZ[j]) != 0)
                {                 
                    return true;
                }
            }
        }

        // All clear
        return false;
    }
}
