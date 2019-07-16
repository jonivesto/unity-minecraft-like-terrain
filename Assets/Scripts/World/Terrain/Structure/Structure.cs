using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure
{
    // true = spawn is relative to ground level
    // false = spawn is relative to bedrock level (y==0)
    public bool spawnFixedToGround = true;

    // Constant displacement from the spawn point
    public int spawnFix = 0;

    // Random displacement from the spawn point
    public int minSpawnAltitude = 0;
    public int maxSpawnAltitude = 191;

    // Biomes this can spawn on
    public int[] spawnBiomes;

    // Array of block types and coords
    // x, y, z, blockID, x, y, z, blockID...
    public int[] model;
}
