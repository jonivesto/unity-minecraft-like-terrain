using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    readonly float x, y, z;

    bool untouched;

    WorldEngine worldEngine;

    string[] blocks = new string[16 * 16 * 16];
    // Item[] items;
    // Mob[] mobs;

    public Chunk(WorldEngine worldEngine, float x, float y, float z)
    {
        this.worldEngine = worldEngine;

        this.x = x;
        this.y = y;
        this.z = z;
        
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
                float point = seed.GetFloat(0);
                int terrainNoise = Mathf.FloorToInt(Mathf.PerlinNoise(x + point, y + point) * 125f);

                for (float t = y; t < y + 16; t++)
                {
                    if(terrainNoise > y)
                    {
                        // empty block
                    }
                    else
                    {
                        // terrain block
                    }
                }
            }
        }
    }

    public void Save()
    {
        if (!untouched)
        {

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
}
