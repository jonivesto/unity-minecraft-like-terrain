﻿public abstract class Item
{
    public abstract string GetName();

    // Returns item's index in Config.ID[]
    public int GetID()
    {
        for(int i = 1; i < Config.ID.Length; i++)
        {
            if (Config.ID[i].Equals(this)) return i;
        }

        return 0;
    }

}
