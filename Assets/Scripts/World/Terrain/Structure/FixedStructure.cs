using System;

public class FixedStructure : Structure
{
    // Array of block types and coords
    // x, y, z, blockID, x, y, z, blockID...
    internal int[] model;

    public override int[] GetModel(Random r)
    {
        return model;
    }
}
