using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class NodeEvent
{
    protected Sprite sprite;

    public abstract void OnVisit();

    public abstract Sprite GetSprite();
}
