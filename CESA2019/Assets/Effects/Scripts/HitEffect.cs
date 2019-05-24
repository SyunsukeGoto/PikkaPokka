//
// HitEffect.cs
// Actor: Tamamura Shuuki
//

using System;
using System.Collections.Generic;
using UnityEngine;


namespace Tama
{
    // プレイヤーが頭をたたいた時のエフェクト
    public class HitEffect : MonoBehaviour
    {

        // エフェクト配列
        [SerializeField, Header("当たったときのエフェクトを設定してください(3段階)")]
        private List<ParticleSystem> _effectList = new List<ParticleSystem>();

        // ヒットレベル2の再生用 
        private float _currentScale;
        private float _endScale;
        private Func<bool> _playLevel2;


        private void Start()
        {
            // ヒットレベル2の初期化
            _effectList[1].transform.localScale = new Vector3(0, 0, 1);
            _currentScale = 0;
            _endScale = 2;
            _playLevel2 = (() => { return true; });
        }
        private void Update()
        {
            _playLevel2();
        }

        // -----------------------------------------------------------
        // エフェクトを再生する
        // force: 当たった時の強さ、3段階（1～3）で設定
        // -----------------------------------------------------------
        public void Play(int force)
        {
            // 強さを3段階に再設定する、強さが範囲外だと困るので!
            force = Mathf.Max(1, Mathf.Min(force, 3));

            // 参照に一つでもNULLがあったら処理を止める
            foreach (var effect in _effectList)
            {
                if (effect == null) return;
            }

            // エフェクトを強さに応じて再生する
            if (force >= 1) _effectList[0].Play();
            if (force >= 2)
            {
                _effectList[1].Play();
                _playLevel2 = (() => {

                    _currentScale += 0.1f;
                    _effectList[1].transform.localScale = new Vector3(_currentScale, _currentScale, 1);
                    if(_currentScale >= _endScale)
                    {
                        _effectList[1].Stop();
                        Start();
                    }
                    return true;
                });
            }
            if (force >= 3) _effectList[2].Play();
        }
    }
}