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
    private PlayCamera _playCamera;


    private void Start()
    {
        // アクターマネージャーを作成
        _actorManager = new ActorManager();
        _actorManager.Initialize();

        // プレイヤーが所持しているメインカメラを参照
        Player player = _actorManager.GetActor(ActorType.PLAYER) as Player;
        //_playCamera = Camera.main.GetComponent<PlayCamera>();
    }

    private void Update()
    {
        // 生成後のカメラをアタッチ
        if (_playCamera == null)
        {
            Camera main = Camera.main;
            if(main != null)
                _playCamera = main.GetComponent<PlayCamera>();
        }

        ExecuteOrder();
        _actorManager.Update();
    }

    // ----------------------------------------------
    // 各マネージャーに命令を伝える
    // ----------------------------------------------
    private void ExecuteOrder()
    {
        if (_actorManager.OnChasing)
        {
            _playCamera.OnTerribly();
        }
        else
        {
            _playCamera.Exit();
        }
    }

    #region プロパティ
    public ActorManager ActorManager { get { return _actorManager; } }
    #endregion
}
