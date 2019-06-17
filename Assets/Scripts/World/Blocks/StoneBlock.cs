using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBlock : BlockComposer
{
    public static readonly int id = 1;

    public override bool SeamlessAndOpaque { get { return true; } }
}
