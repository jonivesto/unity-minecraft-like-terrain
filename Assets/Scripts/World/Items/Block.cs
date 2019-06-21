using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockTransparency
{
              // example:
    Opaque,   // stone
    HideNext, // glass
    ShowNext  // leaf
}

public abstract class Block : Item
{
    protected Vector2[] UVs;
    protected string blockName;
    protected BlockTransparency blockTransparency;


    public override string GetName()
    {
        return blockName;
    }

    public BlockTransparency GetTransparency()
    {
        return blockTransparency;
    }

    public Vector2 GetUV(byte side)
    {
        // For blocks with all sides same texture
        if (UVs.Length == 1)
        {
            return UVs[0];
        }
        // For Blocks that have different texture for each side
        else
        {
            return UVs[side];
        }
    }

}
