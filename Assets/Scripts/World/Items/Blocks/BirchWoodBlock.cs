using UnityEngine;

public class BirchWoodBlock : Block
{
    public BirchWoodBlock()
    {
        blockName = "Birch log";
        blockTransparency = BlockTransparency.Opaque;

        UVs = new Vector2[] {
            new Vector2(4,15), // Top
            new Vector2(4,15), // Bottom
            new Vector2(3,15), // Right
            new Vector2(3,15), // Left
            new Vector2(3,15), // Front
            new Vector2(3,15)  // Back
        };
    }

}