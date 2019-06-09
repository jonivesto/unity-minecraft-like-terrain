using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    readonly int x, y, z;

    WorldEngine worldEngine;

    int[,,] blocks = new int[16, 16, 16];

    public Chunk(WorldEngine worldEngine, float x, float y, float z)
    {
        this.worldEngine = worldEngine;

        this.x = Mathf.FloorToInt(x);
        this.y = Mathf.FloorToInt(y);
        this.z = Mathf.FloorToInt(z);
  
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

    private void Generate()
    {
        Seed seed = worldEngine.seed;

        for (float i = x; i < x + 16; i++)
        {
            for (float j = z; j < z + 16; j++)
            {
                int terrainNoise = Mathf.FloorToInt(Mathf.PerlinNoise(x , y) * 125f);

                for (float t = y; t < y + 16; t++)
                {
                    if(terrainNoise > t)
                    {
                        // empty block
                        blocks[x, y, z] = 0;
                    }
                    else
                    {
                        // terrain block
                        blocks[x, y, z] = 1;
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
        // Save to file
        // Overwrite if file exists
    }


    bool FileExists()
    {
        return false;
    }

    

}
