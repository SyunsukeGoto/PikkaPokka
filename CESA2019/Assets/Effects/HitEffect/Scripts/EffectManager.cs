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
    }

    void PlayHitEffect(int force)
    {
        _hitEffect.Play(force);
    }
}
