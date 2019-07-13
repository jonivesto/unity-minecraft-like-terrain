using System;
using UnityEngine;

public class ChunkTransform
{
    public readonly int x, z;

    const int SIZE_X = 16, SIZE_Z = 16;
    const int SIZE_Y = 256;

    public ChunkTransform(int chunkX, int chunkZ)
    {
        x = chunkX;
        z = chunkZ;
    }

    public ChunkTransform(Vector2Int position)
    {
        x = position.x;
        z = position.y;
    }

    public ChunkTransform(float blockX, float blockZ)
    {     
        x = Mathf.FloorToInt(blockX / SIZE_X);
        z = Mathf.FloorToInt(blockZ / SIZE_Z);  
    }

    public bool PositionEquals(int x, int z)
    {
        return this.x == x && this.z == z;
    }

    public Vector3 GetBlockPosition()
    {
        return new Vector3(x * 16, 0f, z * 16);
    }

    public Vector2Int GetChunkPosition()
    {
        return new Vector2Int(x, z);
    }

    public new string ToString()
    {
        return x + ", " + z;
    }

    internal ChunkTransform GetRight()
    {
        return new ChunkTransform(x + 1, z);
    }

    internal ChunkTransform GetLeft()
    {
        return new ChunkTransform(x - 1, z);
    }

    internal ChunkTransform GetFront()
    {
        return new ChunkTransform(x, z + 1);
    }

    internal ChunkTransform GetBack()
    {
        return new ChunkTransform(x, z - 1);
    }
}
