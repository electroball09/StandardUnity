using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosaceSettings : ScriptableObject
{
    public enum PhysicsSimMode
    {
        NoSim,
        AutoSim,
        UnitySim
    }

    public PhysicsSimMode physicsSimMode;
}
