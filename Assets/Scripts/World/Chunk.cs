﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

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
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if (blocks[x, y, z] == 0) continue; // SKIP IF BLANK AIR

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

                            uvs = AddUvs(uvs, blocks[x, y, z], 0);
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

                            uvs = AddUvs(uvs, blocks[x, y, z], 1);
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

                            uvs = AddUvs(uvs, blocks[x, y, z], 2);
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

                            uvs = AddUvs(uvs, blocks[x, y, z], 3);
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

                            uvs = AddUvs(uvs, blocks[x, y, z], 4);
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

                            uvs = AddUvs(uvs, blocks[x, y, z], 5);
                        }

                    }

                }
            }
        }

        Vector3[] vertices = verts.ToArray();
        int[] triangles = tris.ToArray();
        Vector2[] mapping = uvs.ToArray();


        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = mapping;
        mesh.Optimize();
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material = new Material(Resources.Load<Material>("Textures/Blocks"));
    }

    /*
     * 0 - top
     * 1 - bottom
     * 2 - right
     * 3 - left
     * 4 - front
     * 5 - back
     */
    private List<Vector2> AddUvs(List<Vector2> uvs, int id, byte side)
    {
        //TODO: Get info from block and ad UVs to its coords
        // dont forget sides
        
        float uv = 0.0625f;

        float x = uv * 8;
        float y = uv * 9;


        
        uvs.Add(new Vector2(x, x)); // left bottom
        uvs.Add(new Vector2(x, y)); // left top
        uvs.Add(new Vector2(y, y)); // right top
        uvs.Add(new Vector2(y, x)); // right bottom
        

        return uvs;
    }

    private List<int> AddTriangles(List<int> triangles, int vertices, bool clockwise)
    {
        byte[] pattern;

        if (!clockwise)
        {
            pattern = new byte[] { 0, 1, 2, 0, 2, 3 };
        }
        else
        {
            pattern = new byte[] { 0, 2, 1, 0, 3, 2 };
        }

        for (int i = 0; i < pattern.Length; i++)
        {
            triangles.Add(vertices + pattern[i]);
        }

        return triangles;
    }
}
