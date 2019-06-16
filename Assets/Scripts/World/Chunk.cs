using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    Seed seed;

    int[,,] blocks = new int[16, 256, 16];

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
                    if(blocks[x, y, z] != 0 && blocks[x, y+1, z]==0)
                    {
                        verts.Add(new Vector3(x, y, z));
                        verts.Add(new Vector3(x+1, y, z));
                        verts.Add(new Vector3(x+1, y, z+1));
                        verts.Add(new Vector3(x, y, z+1));

                        tris.Add(verts.Count - 4 + 0);
                        tris.Add(verts.Count - 4 + 2);
                        tris.Add(verts.Count - 4 + 1);

                        tris.Add(verts.Count - 4 + 0);
                        tris.Add(verts.Count - 4 + 3);
                        tris.Add(verts.Count - 4 + 2);
                    }
                }
            }
        }
        Vector3[] vertices = verts.ToArray();
        int[] triangles = tris.ToArray();

        /*Vector3[] vertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0, 1, 1),
            new Vector3 (1, 1, 1),
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1),
        };

        int[] triangles = {
            0, 2, 1, //face front
			0, 3, 2,
            2, 3, 4, //face top
			2, 4, 5,
            1, 2, 5, //face right
			1, 5, 6,
            0, 7, 4, //face left
			0, 4, 3,
            5, 4, 7, //face back
			5, 7, 6,
            0, 6, 7, //face bottom
			0, 1, 6
        };*/

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }
}
