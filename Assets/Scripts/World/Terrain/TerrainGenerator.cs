using NoiseTest;
using System.Linq;
using UnityEngine;
using System;

public class TerrainGenerator
{
    const int CHUNK_X = 16;
    const int CHUNK_Y = 256;
    const int CHUNK_Z = 16;

    TerrainEngine terrainEngine;
    Seed seed;
    Chunk chunk;
    OpenSimplexNoise baseHeightMap, hillsHeightMap, hillsMap, humidityMap, temperatureMap;

    private int worldX, worldZ;   

    public TerrainGenerator(TerrainEngine terrainEngine)
    {
        this.terrainEngine = terrainEngine;
        seed = terrainEngine.seed;

        // Noise maps
        hillsMap = new OpenSimplexNoise(seed.ToLong() / 2 + 1);
        humidityMap = new OpenSimplexNoise(seed.ToLong() / 2);     
        baseHeightMap = new OpenSimplexNoise(seed.ToLong());
        hillsHeightMap = new OpenSimplexNoise(seed.ToLong() / 3 + 1);
        temperatureMap = new OpenSimplexNoise(seed.ToLong() / 3);

    }

    private void SetChunk(Chunk chunk)
    {
        this.chunk = chunk;
        worldX = chunk.chunkTransform.x * CHUNK_X;
        worldZ = chunk.chunkTransform.z * CHUNK_Z;
    }

    public void Generate(Chunk chunk)
    {
        SetChunk(chunk);
        
        for (int x = 0; x < CHUNK_X; x++) // local x
        {
            for (int z = 0; z < CHUNK_Z; z++) // local z
            {
                int ground = GetGroundAt(x + worldX, z + worldZ);
                Biome biome = GetBiomeAt(x + worldX, z + worldZ, ground);

                for (int y = 0; y < CHUNK_Y; y++) // local y
                {
                    if (ground < y)
                    {
                        // Water
                        if (y < Config.SEA_LEVEL)
                        {
                            chunk.SetBlock(x, y, z, 5); // Water
                        }
                        else
                        {
                            chunk.SetBlock(x, y, z, 0); // Empty block
                        }
                    }

                    else
                    {
                        if (y == 0)
                        {
                            chunk.SetBlock(x, y, z, 2); // Bedrock block 
                        }
                        else
                        {
                            chunk.SetBlock(x, y, z, 1); // Stone block 
                        }
                       
                    }
                  

                    // Replace with biome surface material
                    for (int s = 0; s < biome.surfaceDepth; s++)
                    {
                        if(y == ground - s)
                        {
                            chunk.SetBlock(x, y, z, biome.surfaceBlock); // Biome surcafe block
                        }
                    }

                }
            }
        }

    }

    public void Decorate(Chunk chunk)
    {
        SetChunk(chunk);

        // Pick a random starting point
        System.Random r = seed.ChunkBuild(chunk.chunkTransform);
        int x = r.Next(16);
        int z = r.Next(16);
        int ground = GetGroundAt(x + worldX, z + worldZ);
        Biome biome = GetBiomeAt(x, z, ground);

        // Dont make trees on flat
        if (GetHillsAt(x + worldX, z + worldZ) == 0) return;

        // If picked point biome can grow trees
        for (int id = 0; id < biome.treesPerChunk; id++)
        {
            ground = GetGroundAt(x + worldX, z + worldZ);
            biome = GetBiomeAt(x, z, ground);

            int block = chunk.GetBlock(x, ground, z);
            int treeHeight = r.Next(biome.minTreeHeight, biome.maxTreeHeight);
            

            // If picked point is grass or dirt (IDs: 9, 8)
            if (block == 9 || block == 8)
            {
                /*
                // Check surrounding blocks
                // Calcel this three if there is no free space
                int[] patternX = new int[] {-1,-1,-1,0,1,1, 1, 0, 0,0,-2,2};
                int[] patternZ = new int[] {-1, 0, 1,1,1,0,-1,-1,-2,2, 0,0 };

                bool dontCreate = false;

                for (int i = 4; i < treeHeight + 1; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if(x + patternX[j] < 0 || x + patternX[j] > 15){
                            dontCreate = true;
                            break;
                        }
                        if (z + patternZ[j] < 0 || z + patternZ[j] > 15){
                            dontCreate = true;
                            break;
                        }

                        if (chunk.GetBlock(x + patternX[j], ground + i, z + patternZ[j]) != 0)
                        {
                            dontCreate = true;
                        }
                    }
                }
                if (dontCreate)
                {
                    dontCreate = false;
                    continue;
                }
                */

                // Create tree
                for (int i = 1; i < treeHeight; i++)
                {
                    chunk.SetBlock(x, ground + i, z, biome.woodBlock);
                }
                chunk.SetBlock(x, ground + treeHeight, z, biome.leavesBlock);
                //
                //
                int[] leavesX = new int[] { -1, -1, 1, 1, 0,0,-1,1,     -1, -1, 1, 1, 0, 0, -1, 1 ,     -1, -1, 1, 1, 0, 0, -1, 1};
                int[] leavesY = new int[] { 0, 0, 0, 0, 0, 0, 0, 0,     -1, -1, -1, -1, -1, -1, -1, -1,  -2, -2, -2, -2, -2, -2, -2, -2, };
                int[] leavesZ = new int[] { -1, 1, -1, 1, 1,-1,0,0,      -1, 1, -1, 1, 1, -1, 0, 0 ,      -1, 1, -1, 1, 1, -1, 0, 0};
                for (int i = 0; i < leavesX.Length; i++)
                {
                    terrainEngine.WorldSetBlock(x + worldX +leavesX[i], ground + treeHeight + leavesY[i], z + worldZ + leavesZ[i], biome.leavesBlock);
                }
                

            }

            // New position for next tree
            x = r.Next(16);
            z = r.Next(16);
        }
    }

    private int GetGroundAt(int x, int y)
    {

        // This noise defines oceans and continents
        double continentNoise = baseHeightMap.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE) 
                              * 70f // height multiplier
                              + Config.SEA_LEVEL;

        // Define hills
        double hillNoise = GetHillsAt(x, y);

        // Define more detailed terrain
        double terrainNoise = baseHeightMap.Evaluate(x / 20f, y / 20f) 
                            * (hillsHeightMap.Evaluate(x / 300f, y / 300f) * 15f);

        // Combine noise maps for the final result
        return Mathf.FloorToInt((float)(continentNoise + hillNoise + terrainNoise));
    }

    private double GetHillsAt(int x, int y)
    {
        // Get flatness map
        float flatness = Mathf.PerlinNoise(x / Config.FLATNESS, y / Config.FLATNESS);

        double hillNoise = hillsMap.Evaluate(x / Config.HILL_SIZE, y / Config.HILL_SIZE)
                         * flatness
                         * (hillsHeightMap.Evaluate(x / 100f, y / 100f) * 150f);

        if (hillNoise < 0) hillNoise = 0;

        return hillNoise;
    }

    public Biome GetBiomeAt(float x, float y, int ground)
    {
        // beach and ocean
        if (ground < Config.SEA_LEVEL + (Mathf.PerlinNoise(x / 44f, y / 44f) * 3))
        {
            return Config.BIOMES[0]; // Ocean
        }
        else
        {
            return Config.BIOMES[1];
        }
    }

    private Biome GetDominantBiome(Chunk chunk)
    {
        int wx = chunk.chunkTransform.x * CHUNK_X;
        int wz = chunk.chunkTransform.z * CHUNK_Z;

        int[] checks = new int[5];

        checks[0] = GetBiomeAt(0 + wx, 0 + wz, GetGroundAt(0 + wx, 0 + wz)).GetID();
        checks[1] = GetBiomeAt(0 + wx, 15 + wz, GetGroundAt(0 + wx, 15 + wz)).GetID();
        checks[2] = GetBiomeAt(15 + wx, 0 + wz, GetGroundAt(15 + wx, 0 + wz)).GetID();
        checks[3] = GetBiomeAt(15 + wx, 15 + wz, GetGroundAt(15 + wx, 15 + wz)).GetID();
        checks[4] = GetBiomeAt(7 + wx, 7 + wz, GetGroundAt(7 + wx, 7 + wz)).GetID();

        return Config.BIOMES[checks.GroupBy(value => value)
                                .OrderByDescending(group => group.Count()).First().First()];
    }

}
