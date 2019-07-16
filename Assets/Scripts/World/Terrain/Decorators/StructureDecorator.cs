using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoiseTest;

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
                t.terrainEngine.WorldSetBlock(model[0], model[1], model[2], model[3]);
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

        // TODO: check if this location is ok for this structure

        if(t.simplex1.Evaluate(gx, ground, gz) > 0.65)
        {
            // Spawn structure
            for (int i = 0; i < structure.model.Length; i+=4)
            {
                t.terrainEngine.WorldSetBlock(gx + structure.model[i], ground + structure.model[i+1], gz + structure.model[i+2], structure.model[i+3]);
            }
        }

    }
}
