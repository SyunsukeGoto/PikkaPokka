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
    [SerializeField]
    private TutorialManager _tutorialManager = null;


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
            if (this.gameObject.GetComponent<AudioSource>().time == 0.0f && !this.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.gameObject.GetComponent<AudioSource>().clip);
            }
        }
        else
        {
            _playCamera.Exit();
        }

        // チュートリアル状態ならアクターに停止命令を与える
        if (_tutorialManager.Tutorial)
        {
            //_actorManager.GetActor(ActorType.PLAYER).gameObject.SetActive(false);
            _actorManager.GetActor(ActorType.PLAYER).GetComponent<Momoya.PlayerController>()._isActive = false;
        }
        else
        {
            //_actorManager.GetActor(ActorType.PLAYER).gameObject.SetActive(true);
            _actorManager.GetActor(ActorType.PLAYER).GetComponent<Momoya.PlayerController>()._isActive = true;
        }
    }

    #region プロパティ
    public ActorManager ActorManager { get { return _actorManager; } }
    #endregion
}
