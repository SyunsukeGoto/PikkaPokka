//
// ActorManager.cs
// Actor: Tamamura Shuuki
//

using System;
using System.Collections.Generic;
using UnityEngine;


// ゲーム内でアクションを起こすオブジェクトを管理対象としたクラス
public class ActorManager
{

    private List<Actor> _actorList;

    private bool _onChasing;  // お化けがプレイヤーを追跡中


    public void Initialize()
    {
        // リストを初期化する    
        _actorList = new List<Actor>();

        // アクター感知オブジェクトを生成
        ActorInstantiateListener.Instance.Initialize();
        ActorInstantiateListener.Instance.EntryListener(() => {
            List<Actor> actorList = ActorInstantiateListener.Instance.GetActor();
            foreach (var actor in actorList){ AddActor(actor);}
            return true;
        });

        _onChasing = false;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(_onChasing)
            {
                _onChasing = false;
            }
            else
            {
                _onChasing = true;
            }
        }
    }

    // ----------------------------------------------
    // 管理下にアクターを追加する
    // Actor: 追加したいアクター
    // ----------------------------------------------
    public void AddActor(Actor actor)
    {
        _actorList.Add(actor);
    }

    #region プロパティ
    // ----------------------------------------------
    // アクタータイプで指定したアクターデータを返す
    // type: アクタータイプ 
    // ----------------------------------------------
    public Actor GetActor(ActorType type)
    {
        foreach (var actor in _actorList)
        {
            if (actor.Type == type)
            {
                return actor;
            }
        }
        return null;
    }

    public bool OnChasing
    {
        get { return _onChasing; }
    }
    #endregion
}
