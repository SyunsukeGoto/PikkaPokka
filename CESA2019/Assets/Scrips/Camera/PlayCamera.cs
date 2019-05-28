//
// PlayCamera.cs
// Actor: Tamamura Shuuki
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// プレイシーンに使用するカメラ
public class PlayCamera : MonoBehaviour
{

    private Terribly _teriibly;

    
    void Start()
    {
        _teriibly = GetComponent<Terribly>();
    }

    // ----------------------------------------------
    // ドキドキカメラを再生する
    // ----------------------------------------------
    public void OnTerribly()
    {
        _teriibly.Play();
    }

    // ----------------------------------------------
    // カメラに適用しているエフェクトを戻す
    // ----------------------------------------------
    public void Exit()
    {
        _teriibly.Stop();
    }
}
