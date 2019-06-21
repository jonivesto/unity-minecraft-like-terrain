using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBlock : Block
{
    private string blockName = "Stone Block";
    private BlockTransparency blockTransparency = BlockTransparency.Opaque; 

    private Vector2[] UVs = {
        new Vector2(1,15)//, // Top
        //new Vector2(2,15), // Bottom
        //new Vector2(5,15), // Right
        //new Vector2(0,15), // Left
        //new Vector2(4,15), // Front
        //new Vector2(0,15)  // Back
    }; 

    public override string GetName()
    {
        return blockName;
    }

    public override BlockTransparency GetTransparency()
    {
        return blockTransparency;
    }

    public override Vector2 GetUV(byte side)
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
