using UnityEngine;

public class GoldOreBlock : Block
{
    public GoldOreBlock()
    {
        blockName = "Gold ore";
        blockTransparency = BlockTransparency.Opaque;

        UVs = new Vector2[] {
            new Vector2(0,11)
        };
    }

}
