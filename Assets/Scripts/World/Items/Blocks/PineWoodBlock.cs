﻿using UnityEngine;

public class PineWoodBlock : Block
{
    public PineWoodBlock()
    {
        blockName = "Pine";
        blockTransparency = BlockTransparency.Opaque;

        UVs = new Vector2[] {
            new Vector2(4,14), // Top
            new Vector2(4,14), // Bottom
            new Vector2(3,14), // Right
            new Vector2(3,14), // Left
            new Vector2(3,14), // Front
            new Vector2(3,14)  // Back
        };
    }

}