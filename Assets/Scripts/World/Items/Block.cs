using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockTransparency
{
    Opaque,   // (stone)
    HideNext, // (glass)
    ShowNext  // (leaf)
}

public abstract class Block : Item
{

    public abstract BlockTransparency GetTransparency();

    public abstract Vector2 GetUV(byte side);

}
