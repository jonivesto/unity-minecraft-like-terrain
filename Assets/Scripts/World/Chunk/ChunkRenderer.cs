using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer
{
    public void Render(Chunk chunk)
    {
        List<Vector3> blockVerts = new List<Vector3>();
        List<int> blockTris = new List<int>();
        List<Vector2> blockUvs = new List<Vector2>();

        List<Vector3> liquidVerts = new List<Vector3>();
        List<int> liquidTris = new List<int>();
        List<Vector2> liquidUvs = new List<Vector2>();

        

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
                        Liquid liquid = Config.ID[currentBlockId] as Liquid;

                        // Current is liquid
                        #region Calculate liquid
                        if (liquid != null)
                        {
                            // Current liquid is still
                            if (liquid.isStill)
                            {
                                bool renderThisSide = false;

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
                                            renderThisSide = true;
                                        }
                                    }
                                    else // Next is not a block
                                    {
                                        if (next != currentBlockId) // Next is not same liquid
                                        {
                                            renderThisSide = true;
                                        }
                                    }
                                }
                                else // Next is out of array bounds
                                {
                                    renderThisSide = true;
                                }

                                // Top face
                                if (renderThisSide)
                                {
                                    liquidVerts.Add(new Vector3(x, y + 0.9f, z));
                                    liquidVerts.Add(new Vector3(x + 1, y + 0.9f, z));
                                    liquidVerts.Add(new Vector3(x + 1, y + 0.9f, z + 1));
                                    liquidVerts.Add(new Vector3(x, y + 0.9f, z + 1));

                                    int vCount = liquidVerts.Count - 4;
                                    AddTriangles(liquidTris, vCount, true);

                                    AddUvs(liquidUvs, currentBlockId, 0);

                                    renderThisSide = false;
                                }

                                // Bottom face
                                if (y - 1 >= 0) // Next in array bounds
                                {
                                    int next = chunk.GetBlock(x, y - 1, z);

                                    if (next == 0) // Next is air
                                    {
                                        renderThisSide = true;
                                    }
                                    else if (Config.ID[next] as Block != null) // Next is a block
                                    {
                                        Block nextBlock = Config.ID[next] as Block;

                                        if (nextBlock.GetTransparency() != BlockTransparency.Opaque) // Next block is transparent
                                        {
                                            renderThisSide = true;
                                        }
                                    }
                                    else // Next is not a block
                                    {
                                        if (next != currentBlockId) // Next is not same liquid
                                        {
                                            renderThisSide = true;
                                        }
                                    }
                                }

                                // Bottom face
                                if (renderThisSide)
                                {
                                    liquidVerts.Add(new Vector3(x, y, z));
                                    liquidVerts.Add(new Vector3(x + 1, y, z));
                                    liquidVerts.Add(new Vector3(x + 1, y, z + 1));
                                    liquidVerts.Add(new Vector3(x, y, z + 1));

                                    int vCount = liquidVerts.Count - 4;
                                    AddTriangles(liquidTris, vCount, false);

                                    AddUvs(blockUvs, currentBlockId, 1);

                                    renderThisSide = false;
                                }

                                // TODO: Other faces for liquid


                            }
                            // Current liquid is flowing
                            else
                            {
                                // TODO
                            }
                        }
                        #endregion

                        // Current is not liquid
                        else
                        {
                            Custom custom = Config.ID[currentBlockId] as Custom;

                            // Current is custom item
                            #region Render custom item

                            if (custom != null)
                            {
                                // Create obj and add components
                                GameObject customObj = new GameObject("(" + x + ", " + y + ", " + z + ") " + custom.itemName);
                                customObj.AddComponent<MeshFilter>();
                                customObj.AddComponent<MeshRenderer>();
                                customObj.AddComponent<MeshCollider>();

                                // Set parent and position
                                customObj.transform.SetParent(chunk.transform.GetChild(1));
                                customObj.transform.localPosition = new Vector3(x, y, z);

                                // Load mesh
                                Mesh m = (Mesh)Resources.Load(custom.prefabPath, typeof(Mesh));
                                customObj.GetComponent<MeshFilter>().sharedMesh = m;
                                customObj.GetComponent<MeshCollider>().sharedMesh = m;
                            }
                            #endregion
                        }
                    }

                    // Current is a block
                    #region Calculate block
                    else
                    {
                        bool renderThisSide = false;

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
                        if (renderThisSide)
                        {
                            blockVerts.Add(new Vector3(x, y + 1, z));
                            blockVerts.Add(new Vector3(x + 1, y + 1, z));
                            blockVerts.Add(new Vector3(x + 1, y + 1, z + 1));
                            blockVerts.Add(new Vector3(x, y + 1, z + 1));

                            int vCount = blockVerts.Count - 4;
                            AddTriangles(blockTris, vCount, true);

                            AddUvs(blockUvs, currentBlockId, 0);

                            renderThisSide = false;
                        }


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
                        if (renderThisSide)
                        {
                            blockVerts.Add(new Vector3(x, y, z));
                            blockVerts.Add(new Vector3(x + 1, y, z));
                            blockVerts.Add(new Vector3(x + 1, y, z + 1));
                            blockVerts.Add(new Vector3(x, y, z + 1));

                            int vCount = blockVerts.Count - 4;
                            AddTriangles(blockTris, vCount, false);

                            AddUvs(blockUvs, currentBlockId, 1);

                            renderThisSide = false;
                        }

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
                        if (renderThisSide)
                        {
                            blockVerts.Add(new Vector3(x + 1, y, z));
                            blockVerts.Add(new Vector3(x + 1, y + 1, z));
                            blockVerts.Add(new Vector3(x + 1, y + 1, z + 1));
                            blockVerts.Add(new Vector3(x + 1, y, z + 1));

                            int vCount = blockVerts.Count - 4;
                            AddTriangles(blockTris, vCount, false);

                            AddUvs(blockUvs, currentBlockId, 2);

                            renderThisSide = false;
                        }

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
                            blockVerts.Add(new Vector3(x, y, z));
                            blockVerts.Add(new Vector3(x, y + 1, z));
                            blockVerts.Add(new Vector3(x, y + 1, z + 1));
                            blockVerts.Add(new Vector3(x, y, z + 1));

                            int vCount = blockVerts.Count - 4;
                            AddTriangles(blockTris, vCount, true);

                            AddUvs(blockUvs, currentBlockId, 3);

                            renderThisSide = false;
                        }

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
                        if (renderThisSide)
                        {
                            blockVerts.Add(new Vector3(x, y, z + 1));
                            blockVerts.Add(new Vector3(x, y + 1, z + 1));
                            blockVerts.Add(new Vector3(x + 1, y + 1, z + 1));
                            blockVerts.Add(new Vector3(x + 1, y, z + 1));

                            int vCount = blockVerts.Count - 4;
                            AddTriangles(blockTris, vCount, true);

                            AddUvs(blockUvs, currentBlockId, 4);

                            renderThisSide = false;
                        }

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
                        if (renderThisSide)
                        {
                            blockVerts.Add(new Vector3(x, y, z));
                            blockVerts.Add(new Vector3(x, y + 1, z));
                            blockVerts.Add(new Vector3(x + 1, y + 1, z));
                            blockVerts.Add(new Vector3(x + 1, y, z));

                            int vCount = blockVerts.Count - 4;
                            AddTriangles(blockTris, vCount, false);

                            AddUvs(blockUvs, currentBlockId, 5);

                            renderThisSide = false;
                        }

                    }
                    #endregion

                }
            }
        }

        // Build the main block mesh
        Mesh mesh = chunk.gameObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = blockVerts.ToArray();
        mesh.triangles = blockTris.ToArray();
        mesh.uv = blockUvs.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        chunk.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        MeshRenderer renderer = chunk.gameObject.GetComponent<MeshRenderer>();
        renderer.material = new Material(Resources.Load<Material>("Materials/Blocks"));

        // Build liquids
        GameObject chunkLiquids = chunk.transform.GetChild(0).gameObject;
        mesh = chunkLiquids.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = liquidVerts.ToArray();
        mesh.triangles = liquidTris.ToArray();
        mesh.uv = liquidUvs.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        renderer = chunkLiquids.GetComponent<MeshRenderer>();
        renderer.material = new Material(Resources.Load<Material>("Materials/Liquids"));

        // Display customs
        chunk.transform.GetChild(1).gameObject.SetActive(true);
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

        // Choose block or liquid
        // Get texture coordinates for this side
        Block b = Config.ID[id] as Block;
        Vector2 uv;
        if (b != null)
        {
            uv = b.GetUV(side);
        }
        else
        {
            Liquid l = Config.ID[id] as Liquid;
            uv = l.GetUV(side);
        }
        

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
