
using UnityEngine;

public abstract class Liquid : Item
{
    protected string liquidName;
    protected Vector2[] UVs;
    protected Vector2[] flowUVs;

    public override string GetName()
    {
        return liquidName;
    }

    public Vector2 GetUV(byte side)
    {
        // For blocks with all sides same texture
        if (UVs.Length == 1)
        {
            return UVs[0];
        }
        // For Blocks that have different texture for each side
        else
        {
            return UVs[side];
        }
    }

    public Vector2 GetFlowUV(byte side)
    {
        // For blocks with all sides same texture
        if (UVs.Length == 1)
        {
            return UVs[0];
        }
        // For Blocks that have different texture for each side
        else
        {
            return UVs[side];
        }
    }
}
