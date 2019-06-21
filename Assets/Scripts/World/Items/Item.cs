using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public abstract string GetName();

    public int GetID()
    {
        for(int i = 1; i < Config.ID.Length; i++)
        {
            if (Config.ID[i].Equals(this)) return i;
        }

        return 0;
    }

}
