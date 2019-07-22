using UnityEngine;

public class AirBlock : Block
{
    public AirBlock()
    {
        blockName = "Air";
        blockTransparency = BlockTransparency.HideNext;

        UVs = new Vector2[] { };
    }

}