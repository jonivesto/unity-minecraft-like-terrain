﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPyramidStructure : FixedStructure
{
    public DebugPyramidStructure()
    {
        spawnFix = 14;
        foundationBlock = 2;

        spawnBiomes = new int[]
        {
            1, 2
        };
    }

    public override int[] GetModel()
    {
        List<int> modelList = new List<int>();

        int range = 13;

        for (int y = 0; y < Config.WORLD_HEIGHT; y++)
        {
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    modelList.Add(x);
                    modelList.Add(y);
                    modelList.Add(z);

                    modelList.Add(1);
                }
            }
            range--;
        }

        return modelList.ToArray();
    }
}
