using NoiseTest;
using UnityEngine;

public class TerrainGenerator
{
    const int CHUNK_X = 16;
    const int CHUNK_Y = 256;
    const int CHUNK_Z = 16;


    Seed seed;
    OpenSimplexNoise height, humidity, temperature;

    private TerrainEngine terrainEngine;


    public TerrainGenerator(TerrainEngine terrainEngine)
    {
        this.terrainEngine = terrainEngine;
        seed = terrainEngine.seed;

        // Noise maps
        height = new OpenSimplexNoise(seed.ToLong());
        humidity = new OpenSimplexNoise(seed.ToLong() / 2 + 1);
        temperature = new OpenSimplexNoise(seed.ToLong() / 3 + 1);

    }

    public void Generate(Chunk chunk)
    {
        int worldX = chunk.chunkTransform.x * CHUNK_X;
        int worldZ = chunk.chunkTransform.z * CHUNK_Z;

        for (int x = 0; x < CHUNK_X; x++) // local x
        {
            for (int z = 0; z < CHUNK_Z; z++) // local z
            {
                int ground = GetGroundAt(x, z, worldX, worldZ);

                for (int y = 0; y < CHUNK_Y; y++) // local y
                {
                    if (ground < y)
                    {
                        // Water
                        if (y < Config.SEA_LEVEL)
                        {
                            chunk.SetBlock(x, y, z, 3); // Water block
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


    private int GetGroundAt(int x, int y, int wx, int wy)
    {       
        // Add block position to chunk position
        x += wx;
        y += wy;

        // Get biome
        Biome biome = GetBiomeAt(x, y);

        // This noise defines oceans and continents
        double continentNoise = height.Evaluate(x / Config.CONTINENT_SIZE, y / Config.CONTINENT_SIZE) 
                                * biome.heightMultiplier
                                + Config.SEA_LEVEL;

        // Define more detailed terrain shapes
        double terrainNoise = height.Evaluate(x / 20f, y / 20f) * biome.terrainMultiplier;

        // Set terrain noise on top of the continent noise
        return Mathf.FloorToInt((float)(continentNoise + terrainNoise));
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
            double h = humidity.Evaluate(x, y);
            double t = temperature.Evaluate(x, y);

            //TODO: set biomes with t and h

            return Config.BIOMES[1];
        }
    }
}
