using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    float x, y, z;

    bool isUntouched;

    string[] blocks = new string[16 * 16 * 16];

    public Chunk(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        CheckTouchedState();
    }

    public void Load()
    {
        if (isUntouched)
        {
            // Generate
        }
        else
        {
            // load from file
        }
    }

    public void Save()
    {
        if (!isUntouched)
        {

        }
    }

    void CheckTouchedState()
    {
        //TODO: CHECK IF FILE EXIST AND SET isUntouched
    }
}
