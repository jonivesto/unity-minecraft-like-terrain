using System;
using UnityEngine;

public class ChunkTransform
{
    public readonly int x, z;

    const int SIZE_X = 16, SIZE_Z = 16;
    const int SIZE_Y = 256;

    private const int POPULATION_OFFSET = 7;

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

    public bool PopulatesBlock(int x, int z)
    {
        return PositionEquals
        (
            Mathf.FloorToInt((x - POPULATION_OFFSET) / (float)SIZE_X),
            Mathf.FloorToInt((z - POPULATION_OFFSET) / (float)SIZE_Z)
        );
    }

    public Vector2Int GetBlockCenter()
    {
        return new Vector2Int
        (
            x * SIZE_X + POPULATION_OFFSET,
            z * SIZE_Z + POPULATION_OFFSET
        );
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
