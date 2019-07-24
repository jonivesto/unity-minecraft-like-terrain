using UnityEngine;

public class Water : Liquid
{
    public Water()
    {
        liquidName = "Water";

        UVs = new Vector2[] {
            new Vector2(0,12)
        };
    }
}
