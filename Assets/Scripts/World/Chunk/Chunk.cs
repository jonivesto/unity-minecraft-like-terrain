using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Chunk : MonoBehaviour
{
    internal ChunkTransform chunkTransform;   
    internal ChunkData chunkData;

    public void Init(ChunkTransform chunkTransform)
    {
        this.chunkTransform = chunkTransform;
        chunkData = new ChunkData();
    }

    public void SetBlock(int x, int y, int z, int blockId)
    {
        chunkData.blocks[x, y, z] = blockId;
    }

    public int GetBlock(int x, int y, int z)
    {
        return chunkData.blocks[x, y, z];
    }

}
