using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class RosaceBehaviour : MonoBehaviour, IRosaceUpdate
{
    string _typeName = null;
    private string typeName
    {
        get
        {
            if (string.IsNullOrEmpty(_typeName))
                _typeName = GetType().Name;
            return _typeName;
        }
    }

    bool _enabled = true;

    public bool IsValidUpdater()
    {
        return _enabled;
    }

    protected virtual void OnEnable()
    {
        _enabled = true;
    }

    protected virtual void OnDisable()
    {
        _enabled = false;
    }

    protected virtual void OnDestroy()
    {
        _enabled = false;
    }

    protected virtual void _PostRosaceUpdate(Rosace.RosaceUpdateContext context)
    {
        
    }

    protected virtual void _RosaceUpdate(Rosace.RosaceUpdateContext context)
    {

    }

    public void PostRosaceUpdate(Rosace.RosaceUpdateContext context)
    {
        Profiler.BeginSample(typeName);
        _PostRosaceUpdate(context);
        Profiler.EndSample();
    }

    public void RosaceUpdate(Rosace.RosaceUpdateContext context)
    {
        Profiler.BeginSample(typeName);
        _RosaceUpdate(context);
        Profiler.EndSample();
    }
}
