using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoiseTest;
using System.Linq;

public static class StructureDecorator
{
    public static void Generate(TerrainGenerator t)
    {
        // Check if there are blocks to set in outOfBounds
        string outName = t.chunk.chunkTransform.x + "," + t.chunk.chunkTransform.z;
        if (t.outOfBoundsDecorations.Contains(outName))
        {
            List<int[]> blocks = t.outOfBoundsDecorations[outName] as List<int[]>;

            // Set blocks
            foreach (int[] model in blocks)
            {
                //t.terrainEngine.WorldSetBlock(model[0], model[1], model[2], model[3]);
                t.chunk.SetBlock(model[0], model[1], model[2], model[3]);
            }

            t.outOfBoundsDecorations.Remove(outName);
        }

        System.Random r = t.seed.ChunkBuild(t.chunk.chunkTransform);

        // Pick a local
        int x = r.Next(16);
        int z = r.Next(16);
        
        // World space coords for the random point
        int gx = x + t.worldX;
        int gz = z + t.worldZ;

        // Groud level at this random point
        int ground = t.GetGroundAt(gx, gz);

        // Biome of the random point
        Biome biome = t.GetBiomeAt(gx, gz, ground);

        // Get random structure from list of possible structures for this biome
        Structure structure = Config.STRUCTURES[biome.structures[r.Next(biome.structures.Length)]];

        // Check if this biome is ok for the structure
        if (!structure.spawnBiomes.Contains(biome.GetID())) return;

        // Ground based on structure properties
        // For ground level
        if (structure.spawnFixedToGround)
        {
            ground += structure.spawnFix;            
        }
        // For underground
        else
        {
            ground += Mathf.Abs(structure.spawnFix);
        }

        if (ground < 1) ground = 1;

        if (t.simplex1.Evaluate(gx, gz) > 0.65)
        {
            // Spawn structure
            int[] model = structure.GetModel();
            for (int i = 0; i < model.Length; i+=4)
            {
                int xx = model[i];
                int yy = ground + model[i + 1];
                int zz = model[i + 2];

                // Foundations
                if (model[i + 1] == 0 && structure.foundationBlock != 0)
                {
                    for (int j = ground; j > 0; j--)
                    {
                        int bid = t.terrainEngine.WorldGetBlock(xx + gx, j, zz + gz);
                        if (bid==0||bid==5||bid==22)
                        {
                            t.terrainEngine.WorldSetBlock(xx + gx, j, zz + gz, structure.foundationBlock);
                            continue;
                        }
                    }
                }

                // Flip if negative spawn pos
                if (gx > 0) xx *= -1;
                if (gz > 0) zz *= -1;

                t.terrainEngine.WorldSetBlock(xx + gx, yy, zz + gz, model[i+3]);
            }
        }

    }
}
