//
// RockCrush.cs
// Actor: Tamamura Shuuki
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// いわくだき
public class RockCrush : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem[] _particles = new ParticleSystem[0];


    // ---------------------------------------------
    // 更新
    // ---------------------------------------------
    private void Update()
    {
    }

    // ---------------------------------------------
    // いわくだきを実行する
    // ---------------------------------------------
    public void Execute()
    {
        foreach (var effect in _particles)
        {
            effect.Play();
        }
    }
}
