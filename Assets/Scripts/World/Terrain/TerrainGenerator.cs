using NoiseTest;
using UnityEngine;
using System;
using System.Collections.Generic;

public class TerrainGenerator
{
    const int CHUNK_X = 16;
    const int CHUNK_Y = 256;
    const int CHUNK_Z = 16;

    public TerrainEngine terrainEngine;
    public Seed seed;
    public Chunk chunk;
    public int worldX, worldZ;

    internal OpenSimplexNoise simplex3, simplex4, simplex1, simplex2, simplex5;
    internal List<int> allowOverride = new List<int>();

    


    public TerrainGenerator(TerrainEngine terrainEngine)
    {
        this.terrainEngine = terrainEngine;
        seed = terrainEngine.seed;

        // Noise maps
        simplex1 = new OpenSimplexNoise(seed.ToLong() / 2 + 1);
        simplex2 = new OpenSimplexNoise(seed.ToLong() / 2);     
        simplex3 = new OpenSimplexNoise(seed.ToLong());
        simplex4 = new OpenSimplexNoise(seed.ToLong() / 3 + 1);
        simplex5 = new OpenSimplexNoise(seed.ToLong() / 3);

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
                            int ore = OreDecorator.GetOreAt(this, x + worldX, y, z + worldZ);
                            if(ore == 0)
                            {
                                chunk.SetBlock(x, y, z, 1); // Stone block   
                            }
                            else
                            {
                                chunk.SetBlock(x, y, z, ore); // Ore block  
                            }
                                                                                
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

                // Grass
                if (biome.grass != 0) // If biome has grass
                {
                    // If grass is not blocked and grows on dirt
                    if (chunk.GetBlock(x, ground + 1, z) == 0 && chunk.GetBlock(x, ground, z) == 9)
                    {
                        // Only grass on flat areas
                        if (GetHillsAt(x + worldX, z + worldZ) == 0)
                        {
                            // Noise makes grass more random
                            if(Mathf.PerlinNoise(x / 4f, z / 4f) > biome.grassDensity)
                            {
                                chunk.SetBlock(x, ground + 1, z, biome.grass);
                                if (!allowOverride.Contains(biome.grass))
                                {
                                    allowOverride.Add(biome.grass);
                                }
                            }                         
                        }
                    }
                }
                
            }
        }

    }

    public void Decorate(Chunk chunk)
    {
        SetChunk(chunk);

        // Generate default trees
        TreeDecorator.GenerateTrees(this);
    }

    public int GetGroundAt(int x, int y)
    {
        // This noise defines oceans and continents
        double continentNoise = simplex3.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE) 
                              * 70f // height multiplier
                              + Config.SEA_LEVEL;

        // Define hills
        double hillNoise = GetHillsAt(x, y);

        // Define more detailed terrain
        double terrainNoise = simplex3.Evaluate(x / 20f, y / 20f) 
                            * (simplex4.Evaluate(x / 300f, y / 300f) * 15f);

        // Combine noise maps for the final result
        return Mathf.FloorToInt((float)(continentNoise + hillNoise + terrainNoise));
    }

    public double GetHillsAt(int x, int y)
    {
        // Get flatness map
        float flatness = Mathf.PerlinNoise(x / Config.FLATNESS, y / Config.FLATNESS);

        double hillNoise = simplex1.Evaluate(x / Config.HILL_SIZE, y / Config.HILL_SIZE)
                         * flatness
                         * (simplex4.Evaluate(x / 100f, y / 100f) * 150f);

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

    // Check if block can be rewritten
    // Items like grass can block trees etc..
    // This method allows trees to replace grass
    public bool AllowOverwrite(int itemId)
    {
        if (itemId == 0) return true;

        if (allowOverride.Contains(itemId)) return true;

        return false;
    }
}
