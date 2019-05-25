//
// Actor.cs
// Actor: Tamamura Shuuki
//

using System;
using UnityEngine;


// アクターの種類
public enum ActorType
{
    PLAYER,
    ENEMY,
}

// ゲーム内でアクションを起こすオブジェクト
// 基底クラス
public abstract class Actor : MonoBehaviour
{

    protected string _name;
    protected ActorType _type;


    #region プロパティ
    public virtual string Name{ get { return _name; }}
    public virtual ActorType Type { get { return _type; } }
    #endregion
}
