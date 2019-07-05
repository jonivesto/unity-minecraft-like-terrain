using UnityEngine;

public class PineLeavesBlock : Block
{
    public PineLeavesBlock()
    {
        blockName = "Pine leaves";
        blockTransparency = BlockTransparency.ShowNext;

        UVs = new Vector2[] {
            new Vector2(2,13)
        };
    }

}