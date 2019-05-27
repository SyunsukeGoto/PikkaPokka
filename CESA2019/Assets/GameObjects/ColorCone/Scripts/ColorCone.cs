//
// ColorCone.cs
// Actor: Tamamura Shuuki
//

using System;
using UnityEngine;


// カラーコーンに使用
public class ColorCone : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer _destructionObj = null;  // コーン破壊用マテリアル

    private float _param;     // 破壊パラメータ
    private float _scale;

    private Func<bool> _destruction;    // 破壊関数


    private void Start()
    {
        

        _param = 0;
        _scale = 1;

        _destruction = (() => { return true; });
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Destruction();
        //}

        _destruction(); // 破壊を実行する

        // パラメータを範囲内に調整
        _param = Mathf.Min(1, _param);
        _scale = Mathf.Min(1, Mathf.Max(0, _scale));

        // マテリアルに適用
        _destructionObj.material.SetFloat("_Destruction", _param);
        _destructionObj.material.SetFloat("_Scale", _scale);
    }

    // --------------------------------------------------------------
    // 破壊実行
    // --------------------------------------------------------------
    public void Destruction()
    {
        _destruction = (() => {
            _param += 0.1f;
            if (_param >= 1)
            {
                _scale = 0;
                _destruction = (() => { return true; });
            }

            return true;
        });  
    }
}
