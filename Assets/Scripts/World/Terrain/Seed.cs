﻿using System;

public class Seed
{
    public string origin;
    public Random get;

    public Seed()
    {
        get = new Random();

        for (int i = 0; i < 16; i++)
        {
            origin += get.Next(1, 10);
        }

        Reset();
    }

    public Seed(string value)
    {
        origin = value;
        Reset();
    }

    public void Reset()
    {
        get = new Random(IntegerAt(0));
    }

    public int IntegerAt(int index)
    {
        string temp = "";
        int i = 0;

        while (temp.Length < 8)
        {
            i++;

            if(i > origin.Length)
            {
                i = 0;
            }

            temp += origin[i]; 
        }

        return int.Parse(temp);
    }

    public new string ToString()
    {
        return origin;
    }

    public long ToLong()
    {
        return Convert.ToInt64(origin);
    }

    public Random ChunkBuild(ChunkTransform chunkTransform)
    {
        return new Random(IntegerAt(3) + IntegerAt(1) + chunkTransform.x + chunkTransform.z *2);
    }
}
