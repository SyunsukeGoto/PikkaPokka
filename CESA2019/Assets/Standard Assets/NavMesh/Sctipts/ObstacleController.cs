using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshObstacle))]
public class ObstacleController : MonoBehaviour
{
    NavMeshObstacle m_navObstacle;

    // 光ったときと光が消えたときの当たり判定のサイズ
    public static readonly Vector3 LightUp_Size = new Vector3(5, 5, 5);
    public static readonly Vector3 LightOff_Size = new Vector3(1, 1, 1);

    // 今のナビメッシュ状の当たり判定
    Vector3 m_nowSize;

    // 今の当たり判定と変化後の当たり判定の差(補間用)
    float m_diffSize;

    // どれくらいの速さで変化させるのか
    public static readonly float Speed = 1.2f;

    delegate void ExecuteState();
    ExecuteState m_state;

    // 経過時間
    float m_timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        m_navObstacle = GetComponent<NavMeshObstacle>();
        m_nowSize = LightOff_Size;
        m_state = State_None;
    }

    // Update is called once per frame
    void Update()
    {
        m_state();
        // デバッグ用
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            LightUp();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            LightOff();
        }
    }

    public void LightUp()
    {
        // 光った
        m_state = State_LightUp;
        m_nowSize = m_navObstacle.size;
        m_diffSize = Vector3.Distance(m_nowSize, LightUp_Size);
        m_timeElapsed = 0.0f;
    }

    public void LightOff()
    {
        // 光が消えた
        m_state = State_LightOff;
        m_nowSize = m_navObstacle.size;
        m_diffSize = Vector3.Distance(LightOff_Size, m_nowSize);
        m_timeElapsed = 0.0f;
    }

    public void State_None()
    {
        // 何もしない
    }

    public void State_LightUp()
    {
        m_timeElapsed += Time.deltaTime;

        // ちょっとずつ変化させる
        // 現在の位置
        float present_Location = (m_timeElapsed * Speed) / m_diffSize;

        // オブジェクトの移動
        m_navObstacle.size = Vector3.Lerp(m_nowSize, LightUp_Size, present_Location);

        if (present_Location >= 1.0f)
        {
            m_nowSize = m_navObstacle.size;
            m_state = State_None;
        }
    }

    public void State_LightOff()
    {
        m_timeElapsed += Time.deltaTime;

        // ちょっとずつ変化させる
        // 現在の位置
        float present_Location = (m_timeElapsed * Speed) / m_diffSize;

        // オブジェクトの移動
        m_navObstacle.size = Vector3.Lerp(m_nowSize,LightOff_Size, present_Location);

        if(present_Location <= 0.0f)
        {
            m_nowSize = m_navObstacle.size;
            m_state = State_None;
        }
    }
}
