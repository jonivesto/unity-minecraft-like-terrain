using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkData
{
    public int[,,] blocks = new int[16, 256, 16];

    public ChunkData()
    {
        
    }
}
