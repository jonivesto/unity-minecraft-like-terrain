using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Custom
{
    public Grass()
    {
        itemName = "Grass";
        prefabPath = "Prefabs/Custom/Grass";
        displaced = true;
        collision = false;
    }
}
