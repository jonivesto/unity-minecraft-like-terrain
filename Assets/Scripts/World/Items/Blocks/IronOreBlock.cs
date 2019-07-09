using UnityEngine;

public class IronOreBlock : Block
{
    public IronOreBlock()
    {
        blockName = "Iron ore";
        blockTransparency = BlockTransparency.Opaque;

        UVs = new Vector2[] {
            new Vector2(0,12)
        };
    }

}
