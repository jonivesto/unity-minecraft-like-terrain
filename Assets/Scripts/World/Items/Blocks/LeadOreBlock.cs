using UnityEngine;

public class LeadOreBlock : Block
{
    public LeadOreBlock()
    {
        blockName = "Lead ore";
        blockTransparency = BlockTransparency.Opaque;

        UVs = new Vector2[] {
            new Vector2(1,11)
        };
    }

}
