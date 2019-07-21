using System;
using System.Collections;
using System.Collections.Generic;

public class ProcedualStructure : Structure
{
    internal int generateSize = 24;
    internal ProcedualStructureRoom[] rooms;

    // Generate model
    public override int[] GetModel(Random r)
    {
        List<int> modelList = new List<int>();

        int range = 4;

        for (int y = 0; y < Config.WORLD_HEIGHT; y++)
        {
            for (int x = -range; x <= range; x++)
            {
                for (int z = -range; z <= range; z++)
                {
                    modelList.Add(x);
                    modelList.Add(y);
                    modelList.Add(z);
                    modelList.Add(0);
                }
            }
            range--;
        }
        return modelList.ToArray();

    }

    
}
