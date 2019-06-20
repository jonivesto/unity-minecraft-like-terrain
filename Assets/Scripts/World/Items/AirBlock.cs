using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlock : Block
{
    public static readonly int id = 0;

    public override bool CoverNeighbors { get { return false; } }
}
