﻿using System.Collections.Generic;
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
                    int currentBlockId = chunk.GetBlock(x, y, z);
                    if (currentBlockId == 0) continue; // Skip air blocks (id == 0)
                    Block currentBlock = Config.ID[currentBlockId] as Block;

                    // Current is not a block
                    if (currentBlock == null)
                    {
                        //TODO
                    }

                    // Current is a block
                    else
                    {
                        bool renderThisSide = false;

                        // Top face
                        // Top face
                        // Top face
                        if (y + 1 < 256) // Next in array bounds
                        {
                            int next = chunk.GetBlock(x, y + 1, z);

                            if (next == 0) // Next is air
                            {
                                renderThisSide = true;
                            }
                            else if (Config.ID[next] as Block != null) // Next is a block
                            {
                                Block nextBlock = Config.ID[next] as Block;

                                if (nextBlock.GetTransparency() != BlockTransparency.Opaque) // Next block is transparent
                                {
                                    if (nextBlock.GetTransparency() == BlockTransparency.ShowNext)
                                    {
                                        renderThisSide = true;
                                    }
                                    else if (currentBlockId != next) // Next BlockTransparency.HideNext
                                    {
                                        renderThisSide = true;
                                    }
                                }                                 
                            }
                            else // Next is not a block
                            {
                                renderThisSide = true;
                            }
                        }
                        else // Next is out of array bounds
                        {
                            renderThisSide = true;
                        }

                        // Top face
                        // Top face
                        // Top face
                        if (renderThisSide)
                        {
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x, y + 1, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, true);

                            AddUvs(uvs, currentBlockId, 0);

                            renderThisSide = false;
                        }


                        // Bottom face
                        // Bottom face
                        // Bottom face
                        if (y - 1 >= 0) // Next in array bounds
                        {
                            int nextY = chunk.GetBlock(x, y - 1, z);

                            if (nextY == 0) // Next is air
                            {
                                renderThisSide = true;
                            }
                            else if (Config.ID[nextY] as Block != null) // Next is a block
                            {
                                Block nextBlock = Config.ID[nextY] as Block;

                                if (nextBlock.GetTransparency() != BlockTransparency.Opaque) // Next block is transparent
                                {
                                    if (nextBlock.GetTransparency() == BlockTransparency.ShowNext)
                                    {
                                        renderThisSide = true;
                                    }
                                    else if (currentBlockId != nextY) // Next BlockTransparency.HideNext
                                    {
                                        renderThisSide = true;
                                    }
                                }
                            }
                            else // Next is not a block
                            {
                                renderThisSide = true;
                            }
                        }
                        

                        // Bottom face
                        // Bottom face
                        // Bottom face
                        if (renderThisSide)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x + 1, y, z));
                            verts.Add(new Vector3(x + 1, y, z + 1));
                            verts.Add(new Vector3(x, y, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, false);

                            AddUvs(uvs, currentBlockId, 1);

                            renderThisSide = false;
                        }

                        // Right face     
                        // Right face  
                        // Right face  
                        int nextH = (x + 1 < 16) // Get next from chunk it is in
                            ?chunk.GetBlock(x + 1, y, z)
                            :chunk.nextRight.GetBlock(0, y, z);

                        if (nextH == 0) // Next is air
                        {
                            renderThisSide = true;
                        }
                        else if (Config.ID[nextH] as Block != null) // Next is a block
                        {
                            Block nextBlock = Config.ID[nextH] as Block;

                            if (nextBlock.GetTransparency() != BlockTransparency.Opaque) // Next block is transparent
                            {
                                if (nextBlock.GetTransparency() == BlockTransparency.ShowNext)
                                {
                                    renderThisSide = true;
                                }
                                else if (currentBlockId != nextH) // Next BlockTransparency.HideNext
                                {
                                    renderThisSide = true;
                                }
                            }
                            
                        }
                        else // Next is not a block
                        {
                            renderThisSide = true;
                        }

                        // Right face     
                        // Right face  
                        // Right face  
                        if (renderThisSide)
                        {
                            verts.Add(new Vector3(x + 1, y, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, false);

                            AddUvs(uvs, currentBlockId, 2);

                            renderThisSide = false;
                        }

                        // Left face
                        // Left face
                        // Left face
                        nextH = (x - 1 < 0) // Get next from chunk it is in
                            ? chunk.nextLeft.GetBlock(15, y, z)
                            : chunk.GetBlock(x - 1, y, z);
                            

                        if (nextH == 0) // Next is air
                        {
                            renderThisSide = true;
                        }
                        else if (Config.ID[nextH] as Block != null) // Next is a block
                        {
                            Block nextBlock = Config.ID[nextH] as Block;

                            if (nextBlock.GetTransparency() != BlockTransparency.Opaque) // Next block is transparent
                            {
                                if (nextBlock.GetTransparency() == BlockTransparency.ShowNext)
                                {
                                    renderThisSide = true;
                                }
                                else if (currentBlockId != nextH) // Next BlockTransparency.HideNext
                                {
                                    renderThisSide = true;
                                }
                            }
                            
                        }
                        else // Next is not a block
                        {
                            renderThisSide = true;
                        }


                        if (renderThisSide)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x, y + 1, z + 1));
                            verts.Add(new Vector3(x, y, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, true);

                            AddUvs(uvs, currentBlockId, 3);

                            renderThisSide = false;
                        }

                        // Front face
                        // Front face
                        // Front face    
                        nextH = (z + 1 < 16) // Get next from chunk it is in
                            ? chunk.GetBlock(x, y, z + 1)
                            : chunk.nextFront.GetBlock(x, y, 0);

                        if (nextH == 0) // Next is air
                        {
                            renderThisSide = true;
                        }
                        else if (Config.ID[nextH] as Block != null) // Next is a block
                        {
                            Block nextBlock = Config.ID[nextH] as Block;

                            if (nextBlock.GetTransparency() != BlockTransparency.Opaque) // Next block is transparent
                            {
                                if (nextBlock.GetTransparency() == BlockTransparency.ShowNext)
                                {
                                    renderThisSide = true;
                                }
                                else if (currentBlockId != nextH) // Next BlockTransparency.HideNext
                                {
                                    renderThisSide = true;
                                }
                            }
                            
                        }
                        else // Next is not a block
                        {
                            renderThisSide = true;
                        }

                        // Front face
                        // Front face
                        // Front face    
                        if (renderThisSide)
                        {
                            verts.Add(new Vector3(x, y, z + 1));
                            verts.Add(new Vector3(x, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y + 1, z + 1));
                            verts.Add(new Vector3(x + 1, y, z + 1));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, true);

                            AddUvs(uvs, currentBlockId, 4);

                            renderThisSide = false;
                        }

                        // Back face
                        // Back face
                        // Back face
                        nextH = (z - 1 < 0) // Get next from chunk it is in                         
                            ? chunk.nextBack.GetBlock(x, y, 15)
                            : chunk.GetBlock(x, y, z - 1);

                        if (nextH == 0) // Next is air
                        {
                            renderThisSide = true;
                        }
                        else if (Config.ID[nextH] as Block != null) // Next is a block
                        {
                            Block nextBlock = Config.ID[nextH] as Block;

                            if (nextBlock.GetTransparency() != BlockTransparency.Opaque) // Next block is transparent
                            {
                                if (nextBlock.GetTransparency() == BlockTransparency.ShowNext)
                                {
                                    renderThisSide = true;
                                }
                                else if (currentBlockId != nextH) // Next BlockTransparency.HideNext
                                {
                                    renderThisSide = true;
                                }
                            }
                            
                        }
                        else // Next is not a block
                        {
                            renderThisSide = true;
                        }

                        // Back face
                        // Back face
                        // Back face
                        if (renderThisSide)
                        {
                            verts.Add(new Vector3(x, y, z));
                            verts.Add(new Vector3(x, y + 1, z));
                            verts.Add(new Vector3(x + 1, y + 1, z));
                            verts.Add(new Vector3(x + 1, y, z));

                            int vCount = verts.Count - 4;
                            AddTriangles(tris, vCount, false);

                            AddUvs(uvs, currentBlockId, 5);

                            renderThisSide = false;
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
        const float unit = 0.0625f;

        // Get texture coordinates for this side
        Vector2 uv = ((Block)Config.ID[id]).GetUV(side);

        // Texture coordinates in the 16x16 grid
        float x = uv.x;
        float y = uv.y;

        // Convert to texture units
        x *= unit;
        y *= unit;

        // Illegal coords warning
        if (x > 15 || x < 0 || y > 15 || y < 0)
        {
            Debug.LogWarning("Block's texture coordinates are out of range [0, 15]");
        }

        // Add UVs
        uvs.Add(new Vector2(x, y));                 // left bottom
        uvs.Add(new Vector2(x, y + unit));          // left top
        uvs.Add(new Vector2(x + unit, y + unit));   // right top
        uvs.Add(new Vector2(x + unit, y));          // right bottom
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
