using UnityEngine;

public class Water : Liquid
{
    public Water()
    {
        liquidName = "Water";
        isStill = true;

        UVs = new Vector2[] {
            new Vector2(0,12)
        };
    }
}
