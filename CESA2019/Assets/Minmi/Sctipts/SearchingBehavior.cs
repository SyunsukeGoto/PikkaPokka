using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SearchingBehavior : MonoBehaviour
{
    public event System.Action<GameObject> onFound = (obj) => { };
    public event System.Action<GameObject> onLost = (obj) => { };

    [SerializeField, Range(0.0f, 360.0f)]
    private float m_searchAngle = 0.0f;

    private SphereCollider m_sphereCollider = null;

    public float SearchAngle
    {
        set { m_searchAngle = value; }
        get { return m_searchAngle; }
    }

    public float SearchRadius
    {
        get
        {
            if (m_sphereCollider == null)
            {
                m_sphereCollider = GetComponent<SphereCollider>();
            }
            return m_sphereCollider != null ? m_sphereCollider.radius : 0.0f;
        }
    }

    private void Awake()
    {
        m_sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider i_other)
    {
        GameObject enterObject = i_other.gameObject;
        onFound(enterObject);
    }

    private void OnTriggerExit(Collider i_other)
    {
        GameObject exitObject = i_other.gameObject;
        onLost(exitObject);
    }
}
