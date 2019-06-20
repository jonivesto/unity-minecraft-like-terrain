using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Item
{
    public virtual bool CoverNeighbors { get { return false; } }
}
