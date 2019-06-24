using NoiseTest;
using UnityEngine;

public class TerrainGenerator
{
    const int CHUNK_X = 16;
    const int CHUNK_Y = 256;
    const int CHUNK_Z = 16;

    Seed seed;
    OpenSimplexNoise baseHeightMap, hillsHeightMap, flatnessMap, humidityMap, temperatureMap;


    public TerrainGenerator(TerrainEngine terrainEngine)
    {
        seed = terrainEngine.seed;

        // Noise maps
        flatnessMap = new OpenSimplexNoise(seed.ToLong() / 2 + 1);
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
                }
            }
        }

    }


    private int GetGroundAt(int x, int y)
    {
        // TODO: SOME REWORK HERE
        // 1. Get flatness noise (large noise like the height noise)
        // 2. Get hills noise
        // 3. Use flatness to add hills
        // 4. GetBiomeAt() Use flatness with temperature and humidity to set biomes

        // Get flatness map
        float flatness = Mathf.PerlinNoise(x / Config.FLATNESS, y / Config.FLATNESS);

        // Get biome
        Biome biome = GetBiomeAt(x, y, flatness);

        // This noise defines oceans and continents
        double continentNoise = baseHeightMap.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE) 
                              * biome.heightMultiplier
                              + Config.SEA_LEVEL;

        // Define hills
        double hillNoise = flatnessMap.Evaluate(x / Config.HILL_SIZE, y / Config.HILL_SIZE) * flatness * 60f;
        if (hillNoise < 0) hillNoise = 0;

        // Define more detailed terrain
        double terrainNoise = baseHeightMap.Evaluate(x / biome.terrainResolution, y / biome.terrainResolution) * biome.terrainMultiplier;

        // Combine noise maps for the final result
        return Mathf.FloorToInt((float)(continentNoise + hillNoise + terrainNoise));
    }

    public Biome GetBiomeAt(float x, float y, float f)
    {
        // Pre-calculate continent noise
        double b = baseHeightMap.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE);

        // Negative b will be underwater
        if (b < 0)
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
