//
// ActorInstantiateListener.cs
// Actor: Tamamura Shuuki
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// アクター（ゲーム内でアクションを起こすオブジェクト）が生成されたことを感知するクラス
public class ActorInstantiateListener
{

    private static ActorInstantiateListener _instance;
    private List<Actor> _instanceActorList;
    private Func<bool> _listener = (()=> { return true; });


    ~ActorInstantiateListener()
    {
        _instanceActorList.Clear();
        _listener = (() => { return true; });
    }

    public static ActorInstantiateListener Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ActorInstantiateListener();
            return _instance;
        }
    }

    public void Initialize()
    {
        _instanceActorList = new List<Actor>();
    }

    // -----------------------------------------------------------
    // 観測者の登録
    // listener: Actor生成が感知されたときの処理を記述
    // -----------------------------------------------------------
    public void EntryListener(Func<bool> listener)
    {
        _listener = listener;
    }

    // -----------------------------------------------------------
    // アクターが生成された
    // -----------------------------------------------------------
    public void OnInstantiate(Actor actor)
    {
        _instanceActorList.Add(actor);
        _listener();    // マネージャーに通知する
    }

    // -----------------------------------------------------------
    // アクター情報を取得する
    // return: アクター情報
    // -----------------------------------------------------------
    public List<Actor> GetActor()
    {
        List<Actor> outputList = new List<Actor>();

        foreach (var actor in _instanceActorList)
        {
            outputList.Add(actor);
        }
        _instanceActorList.Clear();

        return outputList;
    }
}
