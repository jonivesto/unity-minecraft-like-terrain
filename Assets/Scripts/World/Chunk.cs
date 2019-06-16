using System;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    Seed seed;

    int[,,] blocks = new int[16, 256, 16];

    void Start()
    {
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = transform.position;
        cylinder.transform.parent = transform;     
    }

    public void SetBlock(int x, int y, int z, int blockId)
    {
        blocks[x, y, z] = blockId;
    }

    public int GetBlock(int x, int y, int z)
    {
        return blocks[x, y, z];
    }

    

    
}
