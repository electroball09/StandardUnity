using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosaceGameObject : RosaceBehaviour
{
    void Awake()
    {
        Rosace.RegisterUpdater(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Rosace.DeregisterUpdater(this);
    }

    protected override void _RosaceUpdate(Rosace.RosaceUpdateContext context)
    {
        context.UpdateGameObject(this, gameObject);
    }
}
