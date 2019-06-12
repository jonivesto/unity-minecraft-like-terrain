using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    WorldEngine worldEngine;

    internal readonly int x, y, z;
    internal readonly string name;

    int[,,] blocks = new int[16, 16, 16];

    public Chunk(WorldEngine worldEngine, float x, float y, float z)
    {
        this.worldEngine = worldEngine;

        this.x = Mathf.FloorToInt(x);
        this.y = Mathf.FloorToInt(y);
        this.z = Mathf.FloorToInt(z);

        this.name = "chunk_" + this.x + "_" + this.y + "_" + this.z;
    }

    public void Load()
    {
        if (FileExists())
        {
            // Load from file
        }
        else
        {
            Generate();
        }
    }

    void Generate()
    {
        Seed seed = worldEngine.seed;
        seed.Reset();

        // Plain sky and terrain
        for (float i = x; i < x + 16; i++)
        {
            for (float j = z; j < z + 16; j++)
            {
                int terrainNoise = Mathf.FloorToInt(Mathf.PerlinNoise(x + seed.get.Next(64), y + seed.get.Next(64)) * 125f);

                for (float t = y; t < y + 16; t++)
                {
                    if (terrainNoise > t)
                    {
                        // empty block
                        blocks[x, y, z] = 0;
                    }
                    else
                    {
                        // stone block
                        blocks[x, y, z] = 1;
                    }
                }
            }
        }

        // Bedrock
        if (y == 0) {

            for (float i = x; i < x + 16; i++)
            {
                for (float j = z; j < z + 16; j++)
                {
                    blocks[x, 0, z] = 2;

                    if (Mathf.PerlinNoise(x + seed.get.Next(64), y + seed.get.Next(64)) > 0.4)
                    {
                        blocks[x, 1, z] = 2;
                    }
                }
            }

        }

    }

    public void Render()
    {

    }

    public void Save()
    {    
        // TODO
        // Save to file
        // Overwrite if file exists
    }


    bool FileExists()
    {
        return false;
    }

    

}
