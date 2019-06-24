using NoiseTest;
using UnityEngine;

public class TerrainGenerator
{
    const int CHUNK_X = 16;
    const int CHUNK_Y = 256;
    const int CHUNK_Z = 16;


    Seed seed;
    OpenSimplexNoise height, hills, hillHeights, humidity, temperature;

    private TerrainEngine terrainEngine;


    public TerrainGenerator(TerrainEngine terrainEngine)
    {
        this.terrainEngine = terrainEngine;
        seed = terrainEngine.seed;

        // Noise maps
        height = new OpenSimplexNoise(seed.ToLong());
        hills = new OpenSimplexNoise(seed.ToLong() / 2+1);
        hillHeights = new OpenSimplexNoise(seed.ToLong() / 3 + 1);
        humidity = new OpenSimplexNoise(seed.ToLong() / 2);
        temperature = new OpenSimplexNoise(seed.ToLong() / 3);

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
                            //chunk.SetBlock(x, y, z, 5); // Water
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


        // Get biome
        Biome biome = GetBiomeAt(x, y);

        // This noise defines oceans and continents
        double continentNoise = height.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE) 
                                * biome.heightMultiplier
                                + Config.SEA_LEVEL;

        // Define hills
        float multiplier = Mathf.Abs((float)hillHeights.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE));
        double hillNoise = hills.Evaluate(x / Config.HILL_SIZE, y / Config.HILL_SIZE) * multiplier *10f;
        if (hillNoise < 0) hillNoise = 0;

        // Define more detailed terrain shapes
        double terrainNoise = height.Evaluate(x / 20f, y / 20f) * biome.terrainMultiplier;

        // Combine noise maps for the final result
        return Mathf.FloorToInt((float)(continentNoise + hillNoise + terrainNoise));
    }

    public Biome GetBiomeAt(float x, float y)
    {
        // Anything below sea level will be ocean
        if(height.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE) < 0)
        {
            return Config.BIOMES[0];
        }
        else
        {
            //double h = humidity.Evaluate(x, y);
            //double t = temperature.Evaluate(x, y);

            //TODO: set biomes with t and h

            return Config.BIOMES[1];
        }
    }
}
