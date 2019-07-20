using System;

public class Structure
{
    // Foundations spawn under the structure so it doesnt float in the air
    // Default is 0 == no foundations
    public int foundationBlock = 0;

    // Terrain flatness range where spawn in allowed
    public double minSpawnFlatness = -1d;
    public double maxSpawnFlatness = 1d;

    // true = spawn is relative to ground level
    // false = spawn is relative to bedrock level (y==1)
    public bool spawnFixedToGround = true;

    // Constant displacement from the spawn height
    public int spawnFix = 0;

    // Array of block types and coords
    // x, y, z, blockID, x, y, z, blockID...
    public virtual int[] GetModel(Random r)
    {
        return new int[] { };
    }
}
