using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSimulation : MonoBehaviour
{
    //float speed = 0.6f;
    TerrainEngine terrainEngine;
    Chunk chunk;
    Material liquidMaterial;
    Transform chunkLiquids;

    public GameObject lavaFlow, waterFlow;

    void Start()
    {
        terrainEngine = GetComponent<TerrainEngine>();
        //InvokeRepeating("Simulate", speed, speed);
        liquidMaterial = Resources.Load<Material>("Materials/Liquids");
    }

    public void Simulate(Chunk chunk)
    {
        this.chunk = chunk;
        chunkLiquids = chunk.transform.GetChild(0);

        foreach  (int[] liquidSource in chunk.liquidSources)
        {
            int x = chunk.chunkTransform.x * 16 + Mathf.Abs(liquidSource[0]);
            int z = chunk.chunkTransform.z * 16 + Mathf.Abs(liquidSource[2]);
            int y = liquidSource[1];
            int l = liquidSource[3];

            Fall(x, y, z, l);
            Spread(x, y, z, l);
        }
    }

    private void Fall(int x, int y, int z, int liquidId)
    {
        if (y <= 0) return;

        int i = 1;
        while (terrainEngine.WorldGetBlock(x, y - i, z) == 0)
        {
            AddLiquid(x, y - i, z, liquidId, 1f);
            i++;
        }

        if(i > 1) // Fall at least one block
        {
            Spread(x, y - i + 1, z, liquidId);
        }
    }

    private void Spread(int x, int y, int z, int liquidId)
    {
        if (y <= 0) return;

        // Left
        if (terrainEngine.WorldGetBlock(x-1, y, z) == 0)
        {
            AddLiquid(x - 1, y, z, liquidId, 0.5f);
            Fall(x-1, y, z, liquidId);
        }

        // Right
        if (terrainEngine.WorldGetBlock(x + 1, y, z) == 0)
        {
            AddLiquid(x + 1, y, z, liquidId, 0.5f);
            Fall(x+1, y, z, liquidId);
        }

        // Back
        if (terrainEngine.WorldGetBlock(x, y, z - 1) == 0)
        {
            AddLiquid(x, y, z-1, liquidId, 0.5f);
            Fall(x, y, z-1, liquidId);
        }

        // Front
        if (terrainEngine.WorldGetBlock(x, y, z + 1) == 0)
        {
            AddLiquid(x, y, z+1, liquidId, 0.5f);
            Fall(x, y, z+1, liquidId);
        }
    }

    private void AddLiquid(int x, int y, int z, int liquidId, float level)
    {
        if (y <= 0) return;

        //TODO: Detect if two liquids collide

        string name = liquidId + "|" + x + "," + y + "," + z;

        if (chunkLiquids.Find(name)==null)
        {
            GameObject prefab = waterFlow;
            if (liquidId == 6){ prefab = lavaFlow; }

            GameObject obj = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
            obj.name = name;
            obj.GetComponentInChildren<MeshRenderer>().material = liquidMaterial;
            obj.transform.SetParent(chunkLiquids);
        }
    }
    
    
}
