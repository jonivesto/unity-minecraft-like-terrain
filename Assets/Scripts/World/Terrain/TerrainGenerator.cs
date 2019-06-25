using NoiseTest;
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

                    Biome biome = GetBiomeAt(x + worldX, z + worldZ, ground);

                    // Replace with biome surface material
                    for (int s = 0; s < Config.SURFACE_DEPTH; s++)
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
                            * (hillsHeightMap.Evaluate(x / 300f, y / 300f) * 20f);

        // Combine noise maps for the final result
        return Mathf.FloorToInt((float)(continentNoise + hillNoise + terrainNoise));
    }

    public Biome GetBiomeAt(float x, float y, int ground)
    {
        // Negative b will be underwater
        if (ground < Config.SEA_LEVEL)
        {
            return Config.BIOMES[0]; // Ocean
        }
        else
        {
            double h = humidityMap.Evaluate(x, y);
            double t = temperatureMap.Evaluate(x, y);

            //TODO: set biomes with t and h

            return Config.BIOMES[1];
        }
    }
}
