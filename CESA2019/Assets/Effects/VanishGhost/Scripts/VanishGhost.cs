//
// VanishGhost.cs
// Actor: Tamamura Shuuki
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ゴーストをドロンする
public class VanishGhost : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem[] _particles = new ParticleSystem[2];

    [SerializeField]
    private GameObject _owner = null;      // このエフェクトを適用させるオブジェクト


    private void Update()
    {
#if (DEBUG == true)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Execute();
        }
#endif
    }

    // ------------------------------------------------
    // このエフェクトを適用するオブジェクトを設定する
    // ------------------------------------------------
    public void SetOwner(GameObject owner)
    {
        _owner = owner;
    }

    // -------------------------------------------------
    // ドロン!実行
    // -------------------------------------------------
    public void Execute()
    {
        if (_owner != null)
        {
            this.transform.position = _owner.transform.position;
        }
        foreach (var effect in _particles)
        {
            effect.Play();
        }
    }
}
