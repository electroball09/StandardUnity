using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosaceBehaviour : MonoBehaviour, IRosaceUpdate
{
    public bool IsValidUpdater()
    {
        return this && enabled && gameObject && gameObject.activeInHierarchy;
    }

    public virtual void PostRosaceUpdate(Rosace.RosaceUpdateContext context)
    {
        
    }

    public virtual void RosaceUpdate(Rosace.RosaceUpdateContext context)
    {
        
    }
}
