using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosaceBehaviour : MonoBehaviour, IRosaceUpdate
{
    public bool IsValidUpdater()
    {
        return this && gameObject;
    }

    public virtual void PostRosaceUpdate()
    {
        
    }

    public virtual void RosaceUpdate(RosaceUpdateContext context)
    {
        
    }
}
