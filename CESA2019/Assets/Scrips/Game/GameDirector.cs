//
// GameDirector.cs
// Actor: Tamamura Shuuki
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ゲーム監督クラス
public class GameDirector : MonoBehaviour
{

    private ActorManager _actorManager;


    private void Start()
    {
        _actorManager = new ActorManager();
        _actorManager.Initialize();
    }

    private void Update()
    {
        _actorManager.Update();
    }

    #region プロパティ
    public ActorManager ActorManager { get { return _actorManager; } }
    #endregion
}
