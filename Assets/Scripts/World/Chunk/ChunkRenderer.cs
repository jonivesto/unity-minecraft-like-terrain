﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    public void Render(Chunk chunk)
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
                    int currentBlock = chunk.GetBlock(x, y, z);

                    if (currentBlock == 0) continue; // SKIP IF BLANK AIR

                    if (currentBlock != 0)
                    {
                        // Top
                        if (y + 1 < 256 && chunk.GetBlock(x, y + 1, z) == 0)
                        {
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x, y + 1, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, true);

                            AddUvs(uvs, currentBlock, 0);
                        }


                        // Bottom
                        if (y == 0 || y - 1 >= 0 && chunk.GetBlock(x, y - 1, z) == 0)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x + 1, y, z));
                            verts.Add(new Vector3(x + 1, y, z + 1));
                            verts.Add(new Vector3(x, y, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, false);

                            AddUvs(uvs, currentBlock, 1);
                        }

                        // Right
                        bool render = x + 1 < 16 && chunk.GetBlock(x + 1, y, z) == 0;
                        if (x + 1 == 16 || render)
                        {
                            verts.Add(new Vector3(x + 1, y, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, false);

                            AddUvs(uvs, currentBlock, 2);
                        }

                        // Left
                        render = x - 1 >= 0 && chunk.GetBlock(x - 1, y, z) == 0;
                        if (x - 1 < 0 || render)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x, y + 1, z + 1));
                            verts.Add(new Vector3(x, y, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, true);

                            AddUvs(uvs, currentBlock, 3);
                        }

                        // Front
                        render = z + 1 < 16 && chunk.GetBlock(x, y, z + 1) == 0;
                        if (z + 1 == 16 || render)
                        {
                            verts.Add(new Vector3(x, y, z + 1));
                            verts.Add(new Vector3(x, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, true);

                            AddUvs(uvs, currentBlock, 4);
                        }

                        // Back
                        render = z - 1 >= 0 && chunk.GetBlock(x, y, z - 1) == 0;
                        if (z - 1 < 0 || render)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y, z));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, false);

                            AddUvs(uvs, currentBlock, 5);
                        }

                    }

                }
            }
        }

        Vector3[] vertices = verts.ToArray();
        int[] triangles = tris.ToArray();
        Vector2[] mapping = uvs.ToArray();


        Mesh mesh = chunk.gameObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = mapping;
        mesh.Optimize();
        mesh.RecalculateNormals();

        chunk.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        MeshRenderer renderer = chunk.gameObject.GetComponent<MeshRenderer>();
        renderer.material = new Material(Resources.Load<Material>("Materials/Terrain"));
    }

    /*
     * @param side:
     * 0 - top
     * 1 - bottom
     * 2 - right
     * 3 - left
     * 4 - front
     * 5 - back
     */
    private void AddUvs(List<Vector2> uvs, int id, byte side)
    {       
        // One texture unit (1/16=0.0625)
        const float uv = 0.0625f;

        // Get texture coordinates for this side
        Vector2 thisBlockSide = Item.GetBlockUV(id, side);////////////////////TODO

        // Texture coordinates in the 16x16 grid
        float x = thisBlockSide.x;
        float y = thisBlockSide.y;

        // Convert to texture units
        x *= uv;
        y *= uv;

        // Illegal coords warning
        if (x > 15 || x < 0 || y > 15 || y < 0)
        {
            Debug.LogWarning("Block's texture coordinates are out of range [0, 15]");
        }

        // Add UVs
        uvs.Add(new Vector2(x, y));             // left bottom
        uvs.Add(new Vector2(x, y + uv));        // left top
        uvs.Add(new Vector2(x + uv, y + uv));   // right top
        uvs.Add(new Vector2(x + uv, y));        // right bottom
    }

    private void AddTriangles(List<int> triangles, int vertices, bool clockwise)
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
    }
}
