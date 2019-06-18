using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    Seed seed;

    internal int[,,] blocks = new int[16, 256, 16];
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

                            int vCount = verts.Count - 4;
                            tris = AddTriangles(tris, vCount, true);
                        }
                        
                        
                        // Bottom
                        if (y == 0 || y - 1 >= 0 && blocks[x, y - 1, z] == 0)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x + 1, y, z));
                            verts.Add(new Vector3(x + 1, y, z + 1));
                            verts.Add(new Vector3(x, y, z + 1));

                            int vCount = verts.Count - 4;
                            tris = AddTriangles(tris, vCount, false);
                        }

                        // Right
                        bool render = x + 1 < 16 && blocks[x + 1, y, z] == 0;                       
                        if (x + 1 == 16 || render)
                        {                         
                            verts.Add(new Vector3(x + 1, y, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y, z + 1));

                            int vCount = verts.Count - 4;
                            tris = AddTriangles(tris, vCount, false);
                        }

                        // Left
                        render = x - 1 >= 0 && blocks[x - 1, y, z] == 0;
                        if (x - 1 < 0 || render)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x, y + 1, z + 1));
                            verts.Add(new Vector3(x, y, z + 1));

                            int vCount = verts.Count - 4;
                            tris = AddTriangles(tris, vCount, true);
                        }

                        // Front
                        render = z + 1 < 16 && blocks[x, y, z + 1] == 0;
                        if (z + 1 == 16 || render)
                        {
                            verts.Add(new Vector3(x, y, z + 1));
                            verts.Add(new Vector3(x, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y, z + 1));

                            int vCount = verts.Count - 4;
                            tris = AddTriangles(tris, vCount, true);
                        }

                        // Back
                        render = z - 1 >= 0 && blocks[x, y, z - 1] == 0;
                        if (z - 1 < 0 || render)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y, z));

                            int vCount = verts.Count - 4;
                            tris = AddTriangles(tris, vCount, false);
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
        renderer.material = new Material(Shader.Find("Diffuse"));
    }

    private List<int> AddTriangles(List<int> triangles, int vertices, bool clockwise)
    {
        int[] pattern;

        if (!clockwise)
        {
            pattern = new int[] { 0, 1, 2, 0, 2, 3 };
        }
        else
        {
            pattern = new int[] { 0, 2, 1, 0, 3, 2 };
        }

        for (int i = 0; i < pattern.Length; i++)
        {
            triangles.Add(vertices + pattern[i]);
        }

        return triangles;
    }
}
