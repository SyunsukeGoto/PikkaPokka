//
// EffectManager.cs
// Actor: Tamamura Shuuki
//

using System.Collections.Generic;
using UnityEngine;


// ゲーム中に使用されるエフェクトの管理を行う
public class EffectManager : MonoBehaviour
{

    [SerializeField]
    private HitEffect _hitEffect = null; // 物にぶつかった時のエフェクト


    private void Update()
    {
#if (DEBUG)
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _hitEffect.Play(1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            _hitEffect.Play(2);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _hitEffect.Play(3);
        }
#endif
    }

    void PlayHitEffect(int force)
    {
        _hitEffect.Play(force);
    }
}
