using UnityEngine;

public class CoalOreBlock : Block
{
    public CoalOreBlock()
    {
        blockName = "Coal ore";
        blockTransparency = BlockTransparency.Opaque;

        UVs = new Vector2[] {
            new Vector2(1,12)
        };
    }

}
