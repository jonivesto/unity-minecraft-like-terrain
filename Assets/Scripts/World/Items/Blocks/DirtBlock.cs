using UnityEngine;

public class DirtBlock : Block
{
    public DirtBlock()
    {
        blockName = "Dirt";
        blockTransparency = BlockTransparency.Opaque;

        UVs = new Vector2[] {
            new Vector2(0,15)//, // Top
            //new Vector2(2,15), // Bottom
            //new Vector2(5,15), // Right
            //new Vector2(0,15), // Left
            //new Vector2(4,15), // Front
            //new Vector2(0,15)  // Back
        };
    }

}