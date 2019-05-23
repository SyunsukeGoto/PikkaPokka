using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MapObjectBace : MonoBehaviour
{
    // プレハブ
    public GameObject m_prefab;
    // 自身の実体
    protected GameObject m_inctanceObject = null;

    /// <summary>
    /// オブジェクトの生成
    /// </summary>
    /// <param name="_pos">配置位置</param>
    virtual public GameObject InstanceObject(Vector3 _pos) {
        m_inctanceObject = Instantiate<GameObject>(gameObject);
        m_inctanceObject.transform.position = _pos;

        return m_inctanceObject;
    }

    /// <summary>
    /// 実体の取得
    /// </summary>
    virtual public void SetObject(GameObject _object) {
        m_inctanceObject = _object;
    }
}
