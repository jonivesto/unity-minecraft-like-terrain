using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure
{
    // Foundations spawn under the structure so it doesnt float in the air
    // Default is 0 == no foundations
    public int foundationBlock = 0;

    // true = spawn is relative to ground level
    // false = spawn is relative to bedrock level (y==1)
    public bool spawnFixedToGround = true;

    // Constant displacement from the spawn height
    public int spawnFix = 0;

    // Biomes this can spawn on
    public int[] spawnBiomes;

    // Array of block types and coords
    // x, y, z, blockID, x, y, z, blockID...
    public virtual int[] GetModel()
    {
        return new int[] { 0, 0, 0, 0 };
    }
}
