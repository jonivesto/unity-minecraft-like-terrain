using UnityEngine;

public class Water : Liquid
{
    public Water()
    {
        liquidName = "Water";

        UVs = new Vector2[] {
            new Vector2(0,12)
        };

        flowUVs = new Vector2[] {
            new Vector2(0,11)
        };
    }
}
