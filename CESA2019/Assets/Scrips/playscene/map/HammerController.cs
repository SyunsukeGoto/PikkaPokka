using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    // ハンマーの状態
    Momoya.PlayerController.HammerState m_hammerState = Momoya.PlayerController.HammerState.NONE;
    
    /// <summary>
    /// ハンマーの状態設定
    /// </summary>
    /// <param name="_state"></param>
    public void CallbackHanmaerState(Momoya.PlayerController.HammerState _state) {
        m_hammerState = _state;
    }

    /// <summary>
    /// ハンマーの状態の取得
    /// </summary>
    /// <returns></returns>
    public Momoya.PlayerController.HammerState GetHammerState(){
        return m_hammerState;
    }
}
