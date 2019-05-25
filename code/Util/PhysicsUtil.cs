using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtil
{
    static Collider[] tmpColliders = new Collider[32];

    public static Vector3 CapsulePenetration(Vector3 position, CapsuleCollider collider, int layerMask)
    {
        var points = CalculateCapsuleColliderPoints(position, collider);

        Physics.OverlapCapsuleNonAlloc(points.point1, points.point2, collider.radius, tmpColliders, layerMask);

        Vector3 dir = Vector3.zero;
        Vector3 tmp = Vector3.zero;
        float tmpDist = 0;
        for (int i = 0; i < tmpColliders.Length; i++)
        {
            if (tmpColliders[i] == null)
                break;

            Physics.ComputePenetration(
                collider, points.point2, collider.transform.rotation,
                tmpColliders[i], tmpColliders[i].transform.position, tmpColliders[i].transform.rotation,
                out tmp, out tmpDist);

            DebugExtension.DebugArrow(points.point2 + Vector3.up, tmp * tmpDist, Color.blue, 5f);

            //DebugExtension.DebugPoint(collider.transform.position + tmp * tmpDist, Color.green, duration: 5f);

            dir += tmp * tmpDist;
        }

        return position + dir;
    }

    public static (Vector3 point1, Vector3 point2) CalculateCapsuleColliderPoints(CapsuleCollider collider)
    {
        return CalculateCapsuleColliderPoints(collider.transform.position, collider);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="collider"></param>
    /// <returns>point1 is top of capsule, point2 is bottom</returns>
    public static (Vector3 point1, Vector3 point2) CalculateCapsuleColliderPoints(Vector3 position, CapsuleCollider collider)
    {
        Vector3 point = Vector3.up * (collider.height - collider.radius * 2);
        Vector3 point1 = (position + collider.center) + point;
        Vector3 point2 = (position + collider.center) - point;

        DebugExtension.DebugCapsule(point1, point2, Color.cyan, collider.radius, 5f, false);

        return (point1, point2);
    }
}
