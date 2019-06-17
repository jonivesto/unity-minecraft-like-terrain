using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlock : BlockComposer
{
    public static readonly int id = 0;

    public override bool SeamlessAndOpaque { get { return false; } }
}
