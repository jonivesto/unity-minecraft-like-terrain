using UnityEngine;

public class GrassBlock : Block
{
    public GrassBlock()
    {
        blockName = "Grass Block";
        blockTransparency = BlockTransparency.Opaque;

        UVs = new Vector2[] {
            new Vector2(0,14), // Top
            new Vector2(0,15), // Bottom
            new Vector2(0,13), // Right
            new Vector2(0,13), // Left
            new Vector2(0,13), // Front
            new Vector2(0,13)  // Back
        };
    }

}