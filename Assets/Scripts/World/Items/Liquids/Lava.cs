using UnityEngine;

public class Lava : Liquid
{
    public Lava()
    {
        liquidName = "Lava";
        isStill = true;

        UVs = new Vector2[] {
            new Vector2(1,12)
        };
    }
}
