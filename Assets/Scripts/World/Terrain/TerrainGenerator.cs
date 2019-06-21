using UnityEngine;

public class TerrainGenerator
{
    const int CHUNK_X = 16;
    const int CHUNK_Y = 256;
    const int CHUNK_Z = 16;

    private TerrainEngine terrainEngine;

    public TerrainGenerator(TerrainEngine terrainEngine)
    {
        this.terrainEngine = terrainEngine;
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
                        chunk.SetBlock(x, y, z, 0); // Empty block
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

    private int GetGroundAt(int x, int y, int wX, int wY)
    {
        Seed seed = terrainEngine.seed;
        seed.Reset();
        x += wX;
        y += wY;
        return Mathf.FloorToInt(Mathf.PerlinNoise(x/10f + seed.get.Next(64), y/ 10f + seed.get.Next(64)) * 9f + 9f);       
    }
}
