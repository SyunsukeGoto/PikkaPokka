//
// PlayerEffect.cs
// Actor: Tamamura Shuuki
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// プレイヤーに使用するエフェクト
public class PlayerEffect : MonoBehaviour
{

    [SerializeField]
    private HitEffect _hitEffect = null;    // ハンマーエフェクト

    private Momoya.PlayerController _playerController;


    private void Start()
    {
        _playerController = GetComponent<Momoya.PlayerController>();
    }

    private void Update()
    {

        if(Time.timeScale == 1)
        {

        // ヒットエフェクトを再生
        // 0はHammerStateのNONE
        // ハンマーの先端からエフェクトが出るようになっています
        Momoya.PlayerController.HammerState none = Momoya.PlayerController.HammerState.NONE;
        if (_playerController.DecisionHammerState != (int)none)
        {
            _hitEffect.Play(_playerController.DecisionHammerState);
        }

        }

    }
}
