using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    Seed seed;

    int[,,] blocks = new int[16, 256, 16];
    internal ChunkTransform chunkTransform;

    public void SetBlock(int x, int y, int z, int blockId)
    {
        blocks[x, y, z] = blockId;
    }

    public int GetBlock(int x, int y, int z)
    {
        return blocks[x, y, z];
    }

    internal void Render()
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if(blocks[x, y, z] != 0)
                    {
                        // Top
                        if (y + 1 < 256 && blocks[x, y + 1, z] == 0)
                        {
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x, y + 1, z + 1));

                            int i = verts.Count - 4;
                            tris.Add(i + 0);
                            tris.Add(i + 2);
                            tris.Add(i + 1);

                            tris.Add(i + 0);
                            tris.Add(i + 3);
                            tris.Add(i + 2);
                        }
                        
                        
                        // Bottom
                        if (y - 1 >= 0 && blocks[x, y - 1, z] == 0)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x + 1, y, z));
                            verts.Add(new Vector3(x + 1, y, z + 1));
                            verts.Add(new Vector3(x, y, z + 1));

                            int i = verts.Count - 4;
                            tris.Add(i + 0);
                            tris.Add(i + 1);
                            tris.Add(i + 2);

                            tris.Add(i + 0);
                            tris.Add(i + 2);
                            tris.Add(i + 3);
                        }
                        
                        // Right
                        if (x + 1 < 16 && blocks[x + 1, y, z] == 0)
                        {
                            verts.Add(new Vector3(x + 1, y, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y, z + 1));

                            int i = verts.Count - 4;
                            tris.Add(i + 0);
                            tris.Add(i + 1);
                            tris.Add(i + 2);

                            tris.Add(i + 0);
                            tris.Add(i + 2);
                            tris.Add(i + 3);
                        }
                        
                        // Left
                        if (x - 1 >= 0 && blocks[x - 1, y, z] == 0)
                        {                          
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x, y + 1, z + 1));
                            verts.Add(new Vector3(x, y, z + 1));

                            int i = verts.Count - 4;
                            tris.Add(i + 0);
                            tris.Add(i + 2);
                            tris.Add(i + 1);

                            tris.Add(i + 0);
                            tris.Add(i + 3);
                            tris.Add(i + 2);
                        }
                        
                        // Front
                        if (z + 1 < 16 && blocks[x, y, z + 1] == 0)
                        {
                            verts.Add(new Vector3(x, y, z + 1));
                            verts.Add(new Vector3(x, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y, z + 1));

                            int i = verts.Count - 4;
                            tris.Add(i + 0);
                            tris.Add(i + 2);
                            tris.Add(i + 1);

                            tris.Add(i + 0);
                            tris.Add(i + 3);
                            tris.Add(i + 2);
                        }
                        
                        // Back
                        if (z - 1 >= 0 && blocks[x , y, z - 1] == 0)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y, z));

                            int i = verts.Count - 4;
                            tris.Add(i + 0);
                            tris.Add(i + 1);
                            tris.Add(i + 2);

                            tris.Add(i + 0);
                            tris.Add(i + 2);
                            tris.Add(i + 3);
                        }
                        

                    }


                }
            }
        }
        Vector3[] vertices = verts.ToArray();
        int[] triangles = tris.ToArray();



        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Specular"));
    }

    internal void SetChunkTransform(ChunkTransform chunkTransform)
    {
        this.chunkTransform = chunkTransform;
    }
}
