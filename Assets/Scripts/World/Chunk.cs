using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    readonly int x, y, z;

    bool untouched;

    WorldEngine worldEngine;

    int[,,] blocks = new int[16, 16, 16];

    public Chunk(WorldEngine worldEngine, float x, float y, float z)
    {
        this.worldEngine = worldEngine;

        this.x = Mathf.FloorToInt(x);
        this.y = Mathf.FloorToInt(y);
        this.z = Mathf.FloorToInt(z);

        CheckTouchedState();
    }

    public void Load()
    {
        if (untouched)
        {
            Generate();
        }
        else
        {
            // load from file
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

    public void Save()
    {
        if (!untouched)
        {
            throw new NotImplementedException();
        }
    }

    public void Unload()
    {
        if (!untouched)
        {
            Save();
        }
    }

    void CheckTouchedState()
    {
        //TODO: CHECK IF FILE EXIST AND SET untouched
    }

    public void Render()
    {

    }
}
