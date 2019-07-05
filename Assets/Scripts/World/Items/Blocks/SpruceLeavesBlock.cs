using UnityEngine;

public class SpruceLeavesBlock : Block
{
    public SpruceLeavesBlock()
    {
        blockName = "Spruce leaves";
        blockTransparency = BlockTransparency.ShowNext;

        UVs = new Vector2[] {
            new Vector2(2,14)
        };
    }

}