using NoiseTest;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class TerrainGenerator
{
    public const int CHUNK_SIZE = Config.CHUNK_SIZE;

    public TerrainEngine terrainEngine;
    public Seed seed;
    public Chunk chunk;
    public int worldX, worldZ;

    internal OpenSimplexNoise simplex3, simplex4, simplex1, simplex2, simplex5;
    internal Hashtable outOfBoundsDecorations = new Hashtable(); // Blocks that are generated outside loaded distance
    internal List<int> allowOverride = new List<int>();
    

    public TerrainGenerator(TerrainEngine terrainEngine)
    {
        this.terrainEngine = terrainEngine;
        seed = terrainEngine.seed;

        // Simplex
        simplex1 = new OpenSimplexNoise(seed.ToLong() / 2 + 1);
        simplex2 = new OpenSimplexNoise(seed.ToLong() / 2);     
        simplex3 = new OpenSimplexNoise(seed.ToLong());
        simplex4 = new OpenSimplexNoise(seed.ToLong() / 3 + 1);
        simplex5 = new OpenSimplexNoise(seed.ToLong() / 3);
    }
    
    internal void SetChunk(Chunk chunk)
    {
        this.chunk = chunk;
        worldX = chunk.chunkTransform.x * CHUNK_SIZE;
        worldZ = chunk.chunkTransform.z * CHUNK_SIZE;
    }

    public virtual void Generate(Chunk chunk)
    {
        SetChunk(chunk);
        
        for (int x = 0; x < CHUNK_SIZE; x++) // local x
        {
            for (int z = 0; z < CHUNK_SIZE; z++) // local z
            {
                int ground = GetGroundAt(x + worldX, z + worldZ);
                Biome biome = GetBiomeAt(x + worldX, z + worldZ, ground);

                for (int y = 0; y < Config.WORLD_HEIGHT; y++) // local y
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
                            // Default is stone block
                            int blockSet = 1;

                            // Get cave if caves enabled
                            if (Config.GENERATE_CAVES)
                            {
                                blockSet = CaveDecorator.Evaluate(this, worldX + x, y, worldZ + z); // Stone (1) or air (0)
                            }
                            
                            // Set ores if ores enabled
                            if(Config.GENERATE_ORES && blockSet == 1)
                            {
                                 blockSet = OreDecorator.GetOreAt(blockSet, this, x + worldX, y, z + worldZ); // Ore block                                  
                            }

                            // Set block that is stone or cave (air) or ore
                            chunk.SetBlock(x, y, z, blockSet);
                        }
                    }
                    
                    // Replace with biome surface material
                    for (int s = 0; s < biome.surfaceDepth; s++)
                    {
                        if(y == ground - s)
                        {
                            if(chunk.GetBlock(x, y, z) != 0 || biome.GetID()==0) // Don't cover caves with surface material, also skil sea biome
                            {
                                if(y == ground)
                                {
                                    chunk.SetBlock(x, y, z, biome.surfaceBlock); // Biome surcafe block
                                }
                                else
                                {
                                    chunk.SetBlock(x, y, z, biome.hiddenSurfaceBlock); // Biome hidden surcafe block
                                }                               
                            }                           
                            
                        }

                    }
                }

                // Lava & noise bedrock
                if (chunk.GetBlock(x, 1, z) == 0)
                {
                    if (Mathf.PerlinNoise(x / 1.5f, z / 1.5f) > 0.5)
                    {
                        chunk.SetBlock(x, 1, z, 2); // Bedrock
                    }
                    else
                    {
                        chunk.SetBlock(x, 1, z, 22); // Lava
                    }
                }

                if (chunk.GetBlock(x, 2, z) == 0)
                {
                    chunk.SetBlock(x, 2, z, 22); // Lava
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
                            if(Perlin.Noise(x / 4f, z / 4f) > biome.grassDensity)
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

    public virtual void Decorate(Chunk chunk)
    {
        SetChunk(chunk);

        // Structures
        StructureDecorator.Generate(this);

        // Generate default trees
        TreeDecorator.GenerateTrees(this);
    }

    public virtual int GetGroundAt(int x, int y)
    {
        // This noise defines oceans and continents
        double continentNoise = simplex3.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE) 
                              * 60f // height multiplier
                              + Config.SEA_LEVEL;
        // Define hills
        double hillNoise = GetHillsAt(x, y);

        // Define more detailed terrain
        double terrainNoise = simplex3.Evaluate(x / 20f, y / 20f) 
                            * (simplex4.Evaluate(x / 300f, y / 300f) * 15f);

        // Combine noise maps for the final result
        return Mathf.FloorToInt((float)(continentNoise + hillNoise + terrainNoise));
    }

    public virtual double GetFlatnessAt(int x, int y)
    {
        return Perlin.NoiseDistorted(x / Config.FLATNESS, y / Config.FLATNESS, 2.1f);
    }

    public virtual double GetHillsAt(int x, int y)
    {
        double hillNoise = simplex1.Evaluate(x / Config.HILL_SIZE, y / Config.HILL_SIZE)
                         * (GetFlatnessAt(x, y) + 0.1f)
                         * (simplex4.Evaluate(x / 100f, y / 100f) * Config.HILLS_MULTIPLIER);

        if (hillNoise < 0) hillNoise = 0;

        return hillNoise;
    }

    public virtual Biome GetBiomeAt(float x, float y, int ground)
    {
        // Beach and ocean
        if (ground < Config.SEA_LEVEL + (Perlin.Noise(x / 34f, y / 34f) * 3))
        {
            return Config.BIOMES[0]; // Ocean
        }
        // Land biomes
        else
        {
            if(simplex1.Evaluate(x / Config.BIOME_SIZE, y / Config.BIOME_SIZE) > 0f)
            {
                return Config.BIOMES[1]; // Birch forest
            }              
            else
            {
                return Config.BIOMES[2]; // Pine forest
            }

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
