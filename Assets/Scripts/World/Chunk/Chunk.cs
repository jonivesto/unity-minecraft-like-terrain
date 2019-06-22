using UnityEngine;

public class Chunk : MonoBehaviour
{
    public bool generated = false;
    public bool rendered = false;
    public bool unsaved = false;

    internal Chunk nextRight, nextLeft, nextFront, nextBack;
    internal ChunkData chunkData;
    internal ChunkTransform chunkTransform;   
    

    public void SetNext(Chunk right, Chunk left, Chunk front, Chunk back)
    {
        nextRight = right;
        nextLeft = left;
        nextFront = front;
        nextBack = back;
    }

    public void SetTransform(ChunkTransform chunkTransform)
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
