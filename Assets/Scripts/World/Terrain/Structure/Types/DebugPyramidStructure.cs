﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPyramidStructure : FixedStructure
{
    public DebugPyramidStructure()
    {
        List<int> modelList = new List<int>();

        int range = 15;

        for (int y = 0; y < Config.WORLD_HEIGHT; y++)
        {
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    modelList.Add(x);
                    modelList.Add(y);
                    modelList.Add(z);

                    if(x<range/2&&z<range/2)
                        modelList.Add(2);
                    else
                        modelList.Add(1);
                }
            }
            range--;
        }

        model = modelList.ToArray();
    }
}
