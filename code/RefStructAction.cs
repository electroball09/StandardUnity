using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RefStructAction<T>(ref T obj) where T : struct;
