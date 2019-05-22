using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class GhostAI : MonoBehaviour
{
    [SerializeField]
    float m_speed;  // 速さ

    public Transform[] m_points;
    int m_destPoint;

    Transform m_target;

    NavMeshAgent m_navAgent;

    SearchingBehavior m_searcher;

    [SerializeField]
    // 感知の範囲
    float m_searchRadius = 3.0f;

    [Range(0.0f,360.0f)]
    [SerializeField]
    float m_searchAngle = 0.0f;

    [SerializeField]
    string m_searchingName = "Player";

    [SerializeField]
    string m_avoidingName = "LightBlock";

    // Start is called before the first frame update
    void Start()
    {
        // 子供オブジェクトの生成 索敵オブジェクト
        GameObject searchingObj = new GameObject("Finder");
        searchingObj.transform.parent = this.gameObject.transform;
        searchingObj.transform.localPosition = new Vector3(0, 0, 0);

        SphereCollider col = searchingObj.AddComponent<SphereCollider>();
        col.radius = m_searchRadius;
        col.isTrigger = true;

        m_searcher = searchingObj.AddComponent<SearchingBehavior>();
        m_searcher.onFound += OnFound;
        m_searcher.onLost += OnLost;



        m_navAgent = GetComponent<NavMeshAgent>();

        m_navAgent.speed = m_speed;

        m_navAgent.autoBraking = false;
        if (m_navAgent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            GotoNextPoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_navAgent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            //navMeshAgentの操作
            // エージェントが現目標地点に近づいてきたら、
            // 次の目標地点を選択します
            if (!m_navAgent.pathPending && m_navAgent.remainingDistance < 0.5f)
                GotoNextPoint();
        }


    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返します
        if (m_points.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        m_navAgent.destination = m_points[m_destPoint].position;

        // 配列内の次の位置を目標地点に設定し、
        // 必要ならば出発地点にもどります
        m_destPoint = (m_destPoint + 1) % m_points.Length;
    }

    // 見つけたときの処理
    private void OnFound(GameObject i_foundObject)
    {
        if (i_foundObject.name == m_searchingName)
        {
            m_target = i_foundObject.transform;
            m_navAgent.destination = m_target.position;

            Debug.Log("Find!!");
        }

        if (i_foundObject.name == m_avoidingName)
        {
            m_target = i_foundObject.transform;

            Vector3 dir = i_foundObject.transform.position - transform.position;
            m_navAgent.destination = Vector3.Reflect(dir, Vector3.up);

            Debug.Log("avoid");
        }
    }

    // 見失ったときの処理
    private void OnLost(GameObject i_lostObject)
    {
        if (m_target == null) return;

        if (i_lostObject.name == m_searchingName)
        {
            GotoNextPoint();

            Debug.Log("Lost...");
        }
    }
}
