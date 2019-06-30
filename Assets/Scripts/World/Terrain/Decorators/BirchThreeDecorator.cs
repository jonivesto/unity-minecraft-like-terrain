using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirchThreeDecorator : Decorator
{
    internal override void Decorate(Seed seed, Chunk chunk, int x, int z, int ground)
    {
        if (ground > Config.SEA_LEVEL && Random.Range(0f,1f) < 0.02)
        {
            if(chunk.GetBlock(x,ground,z)==9)
            for (int i = 1; i < 5; i++)
            {
                chunk.SetBlock(x, ground + i, z, 10);
            }
            chunk.SetBlock(x, ground+5, z, 4);
        }


    }
}