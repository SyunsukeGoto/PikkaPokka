//
// ActorManager.cs
// Actor: Tamamura Shuuki
//

using System.Collections.Generic;
using UnityEngine;


// ゲーム内でアクションを起こすオブジェクトを管理対象としたクラス
public class ActorManager
{

    private List<Actor> _actorList;


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
    }

    public void Update()
    {
        Debug.Log("アクター数" + _actorList.Count.ToString());
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
    public List<Actor> ActorList
    {
        get { return _actorList; }
    }
    #endregion
}
