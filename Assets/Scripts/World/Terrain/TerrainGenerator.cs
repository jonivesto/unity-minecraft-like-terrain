using NoiseTest;
using System.Linq;
using UnityEngine;

public class TerrainGenerator
{
    const int CHUNK_X = 16;
    const int CHUNK_Y = 256;
    const int CHUNK_Z = 16;

    Seed seed;
    OpenSimplexNoise baseHeightMap, hillsHeightMap, hillsMap, humidityMap, temperatureMap;


    public TerrainGenerator(TerrainEngine terrainEngine)
    {
        seed = terrainEngine.seed;

        // Noise maps
        hillsMap = new OpenSimplexNoise(seed.ToLong() / 2 + 1);
        humidityMap = new OpenSimplexNoise(seed.ToLong() / 2);     
        baseHeightMap = new OpenSimplexNoise(seed.ToLong());
        hillsHeightMap = new OpenSimplexNoise(seed.ToLong() / 3 + 1);
        temperatureMap = new OpenSimplexNoise(seed.ToLong() / 3);

    }

    public void Generate(Chunk chunk)
    {
        int worldX = chunk.chunkTransform.x * CHUNK_X;
        int worldZ = chunk.chunkTransform.z * CHUNK_Z;

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

        // Trees
        System.Random d = new System.Random(chunk.chunkTransform.x + chunk.chunkTransform.z);
        int rx = d.Next(16), rz = d.Next(16);       
        int rGround = GetGroundAt(rx + worldX, rz + worldZ);
        Biome rBiome = GetBiomeAt(rx, rz, rGround);
        int rBlock = chunk.GetBlock(rx, rGround, rz);

        for (int id = 0; id < rBiome.treesPerChunk; id++)
        {

            rx = d.Next(16);
            rz = d.Next(16);
            rGround = GetGroundAt(rx + worldX, rz + worldZ);
            rBiome = GetBiomeAt(rx, rz, rGround);
            rBlock = chunk.GetBlock(rx, rGround, rz);

            if (rBlock == 9) // 9 = grass
            {
                for (int i = 1; i < 5; i++)
                {
                    chunk.SetBlock(rx, rGround + i, rz, 10);
                }
                chunk.SetBlock(rx, rGround + 5, rz, 4);
            }
        }
        

        


    }

    private int GetGroundAt(int x, int y)
    {
        // Get flatness map
        float flatness = Mathf.PerlinNoise(x / Config.FLATNESS, y / Config.FLATNESS);

        // Get biome
        //Biome biome = GetBiomeAt(x, y);

        // This noise defines oceans and continents
        double continentNoise = baseHeightMap.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE) 
                              * 70f // height multiplier
                              + Config.SEA_LEVEL;

        // Define hills
        double hillNoise = hillsMap.Evaluate(x / Config.HILL_SIZE, y / Config.HILL_SIZE) 
                         * flatness 
                         * (hillsHeightMap.Evaluate(x / 100f, y / 100f) * 150f);

        if (hillNoise < 0) hillNoise = 0;

        // Define more detailed terrain
        double terrainNoise = baseHeightMap.Evaluate(x / 20f, y / 20f) 
                            * (hillsHeightMap.Evaluate(x / 300f, y / 300f) * 14f);

        // Combine noise maps for the final result
        return Mathf.FloorToInt((float)(continentNoise + hillNoise + terrainNoise));
    }

    public Biome GetBiomeAt(float x, float y, int ground)
    {
        // beach and ocean
        if (ground < Config.SEA_LEVEL + (Mathf.PerlinNoise(x / 44f, y / 44f)*3))
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
