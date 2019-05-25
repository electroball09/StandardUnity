using UnityEngine;
using System.Collections;

public class TransformUtil
{
    public static Transform SearchHierarchyForBone(Transform current, string name)
    {
        if (current.name == name)
            return current;

        for (int i = 0; i < current.childCount; ++i)
        {
            Transform found = SearchHierarchyForBone(current.GetChild(i), name);

            if (found != null)
                return found;
        }

        return null;
    }
}
