using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBlock : Block
{
    public static readonly int id = 1;

    public override bool CoverNeighbors { get { return true; } }
}
