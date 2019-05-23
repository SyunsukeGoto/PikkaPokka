using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MapObjectBace
{
    // 変化後の形
    public GameObject m_block;
    public bool m_state = false;

    /// <summary>
    /// ハンマー判定
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionEnter(Collision col)
    {
        if (!m_state && col.gameObject.tag == "hammer")
        {
            var pos = transform;
            // 状態変化
            Destroy(m_inctanceObject);
            // 現在位置の設定
            m_inctanceObject = Instantiate<GameObject>(m_block);
            m_inctanceObject.transform.position = pos.position;
            // 生成済 
            m_state = true;
        }
    }
}
