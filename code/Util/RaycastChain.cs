using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastChain
{
    /// <summary>
    /// The layer mask to use in the Raycast
    /// </summary>
    public LayerMask layerMask
    {
        get { return m_layerMask; }
        set { m_layerMask = value; }
    }
    private LayerMask m_layerMask;

    /// <summary>
    /// The beginning point of the raycast chain
    /// </summary>
    public Vector3 startPos
    {
        get { return m_startPos; }
        set { m_startPos = value; }
    }
    private Vector3 m_startPos;

    /// <summary>
    /// The rotation quaternion by which to rotate the chain
    /// </summary>
    public Vector3 rotation
    {
        get { return m_rotation; }
        set { m_rotation = value; }
    }
    private Vector3 m_rotation
    {
        get { return mp_rotation.eulerAngles; }
        set { mp_rotation = Quaternion.Euler(value); }
    }
    private Quaternion mp_rotation;

    /// <summary>
    /// Set this with a Transform to set the startPos to the Transform's position and the startDir to the Transform's forward facing vector
    /// </summary>
    public Transform baseTransform
    {
        set
        {
            m_startPos = value.position;
            rotation = value.rotation.eulerAngles;
        }
    }

    public RaycastLink[] links
    {
        get { return m_links; }
    }
    private RaycastLink[] m_links;

    #region constructors
    public RaycastChain()
    {
        m_links = new RaycastLink[0];
        m_startPos = Vector3.zero;
        m_rotation = Vector3.zero;
        layerMask = 0;
    }
    public RaycastChain(RaycastLink[] _links) : this()
    {
        m_links = _links;
    }
    public RaycastChain(Vector3 _startPos, Vector3 _rotation) : this()
    {
        m_startPos = _startPos;
        m_rotation = _rotation;
    }
    public RaycastChain(Vector3 _startPos, Vector3 _rotation, LayerMask _layerMask) : this()
    {
        m_startPos = _startPos;
        m_rotation = _rotation;
        m_layerMask = _layerMask;
    }
    public RaycastChain(Vector3 _startPos, Vector3 _rotation, RaycastLink[] _links) : this()
    {
        m_links = _links;
        m_startPos = _startPos;
        m_rotation = _rotation;
    }
    public RaycastChain(Vector3 _startPos, Vector3 _rotation, LayerMask _layerMask, RaycastLink[] _links)
    {
        m_links = _links;
        m_startPos = _startPos;
        m_rotation = _rotation;
        m_layerMask = _layerMask;
    }
    #endregion


    /// <summary>
    /// Starts at index 0 of the raycast chain and raycasts through to the end, using the position and rotation properties4.  Returns a RaycastResult with the raycast information of the first raycast to hit, or the last raycast in the chain
    /// </summary>
    /// <returns></returns>
    public RaycastResult RaycastAll(bool showDebug = true)
    {
        RaycastHit raycastHit;
        RaycastResult result = RaycastResult.none;

        Vector3 currentPos = m_startPos;
        bool didHit = false;
        for (int i = 0; i < m_links.Length; i++)
        {
            didHit = Physics.Raycast(currentPos, mp_rotation * m_links[i].direction, out raycastHit, m_links[i].distance, m_layerMask);
            if (didHit)
            {
                result = new RaycastResult(didHit, currentPos, raycastHit, i);
#if DEBUG
                if (showDebug)
                    Debug.DrawLine(result.origin, result.point, result.hit ? Color.blue : Color.red, 0.1f);
#endif
                return result;
            }
            else
            {
#if DEBUG
                if (showDebug)
                    Debug.DrawRay(currentPos, (mp_rotation * m_links[i].direction) * m_links[i].distance, didHit ? Color.blue : Color.red, 0.1f);
#endif
            }
            currentPos += (mp_rotation * m_links[i].direction) * m_links[i].distance;
        }

        result = new RaycastResult(didHit, currentPos, mp_rotation * m_links[m_links.Length - 1].direction, m_links[m_links.Length - 1].distance, m_links.Length - 1);
        return result;
    }
    public RaycastResult RaycastAll(Vector3 _startPos, Vector3 _startRotation, bool showDebug = true)
    {
        m_startPos = _startPos;
        m_rotation = _startRotation;
        return RaycastAll(showDebug);
    }

    /// <summary>
    /// Raycasts a specific link in the raycast chain.  Simulates raycasting from each link, therefore providing correct positioning and rotation to this raycast
    /// </summary>
    /// <param name="index">The index of the link to raycast</param>
    /// <returns>RaycastResult containing the information of the raycast</returns>
    public RaycastResult RaycastAt(int index)
    {
        Vector3 currentPos = m_startPos;
        for (int i = 0; i < index; i++)
        {
            //here we simulate casting the rays before the specified index, which gives us correct position and rotation to raycast from
            currentPos += (mp_rotation * m_links[i].direction) * m_links[i].distance;
        }

        RaycastHit raycastHit;
        bool didHit = Physics.Raycast(currentPos, mp_rotation * m_links[index].direction, out raycastHit, m_links[index].distance, m_layerMask);
        if (!didHit)
        {
            return new RaycastResult(didHit, currentPos, mp_rotation * m_links[index].direction, m_links[index].distance, index);
        }

        return new RaycastResult(didHit, currentPos, raycastHit, index);
    }
}

[System.Serializable]
public struct RaycastLink
{
    [SerializeField]
    public Vector3 direction;
    [SerializeField]
    public float distance;

    public RaycastLink(Vector3 _dir, float _dist)
    {
        direction = _dir.normalized;
        distance = _dist;
    }
}

[System.Serializable]
public struct RaycastResult
{
    public static RaycastResult none
    {
        get { return new RaycastResult(false, Vector3.zero, new RaycastHit(), -1); }
    }

    public bool hit
    {
        get { return m_hit; }
    }
    [SerializeField]
    private bool m_hit;

    public Vector3 origin
    {
        get { return m_origin; }
    }
    [SerializeField]
    private Vector3 m_origin;

    public Vector3 direction
    {
        get { return m_direction; }
    }
    [SerializeField]
    private Vector3 m_direction;
    
    public Vector3 normal
    {
        get { return m_normal; }
    }
    [SerializeField]
    private Vector3 m_normal;

    public Vector3 point
    {
        get { return m_point; }
    }
    [SerializeField]
    private Vector3 m_point;

    public float distance
    {
        get { return m_distance; }
    }
    [SerializeField]
    private float m_distance;

    public int linkIndex
    {
        get { return m_linkIndex; }
    }
    [SerializeField]
    private int m_linkIndex;

    public RaycastResult(bool _hit, Vector3 _origin, Vector3 _direction, float _distance, Vector3 _point, Vector3 _normal, int _linkIndex)
    {
        m_hit = _hit;
        m_origin = _origin;
        m_direction = _direction;
        m_distance = _distance;
        m_point = _point;
        m_normal = _normal;
        m_linkIndex = _linkIndex;
    }
    public RaycastResult(bool _hit, Vector3 _origin, RaycastHit _raycastHit, int _linkIndex) 
        : this(_hit, _origin, (_raycastHit.point - _origin).normalized, (_raycastHit.point - _origin).magnitude, _raycastHit.point, _raycastHit.normal, _linkIndex) { }
    public RaycastResult(bool _hit, Vector3 _origin, Vector3 _direction, float _distance, int _linkIndex)
        : this(_hit, _origin, _direction, _distance, _origin + (_direction * _distance), Vector3.up, _linkIndex) { }
}