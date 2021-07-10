using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosaceGameObject : RosaceBehaviour
{
    void Awake()
    {
        Rosace.RegisterUpdater(this);
    }

    private void OnDestroy()
    {
        Rosace.DeregisterUpdater(this);
    }

    public override void RosaceUpdate(Rosace.RosaceUpdateContext context)
    {
        context.UpdateGameObject(this, gameObject);
    }
}
