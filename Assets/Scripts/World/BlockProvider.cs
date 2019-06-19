using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockProvider
{
    public virtual bool CoverNeighbors { get { return true; } }

    public virtual Material TextureMap { get { return new Material(Shader.Find("Diffuse")); } }

}
