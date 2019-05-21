using UnityEngine;
using System.Collections;
using System;

namespace Momoya
{

    //プレイヤーのステート
    namespace PlayerState
    {

        //ステートの実行を管理するクラス
        public class StateProcessor
        {
            //ステート本体
            private PlayerState _state;
            // ステートを取得、セットをするプロパティ
            public PlayerState State
            {
                set { _state = value; }
                get { return _state; }
            }

            // 実行関数
            public void Execute()
            {
                State.Execute();
            }
        }

        //ステートのクラス
        public abstract class PlayerState
        {
            //委譲
            public delegate void ExecuteState();
            public ExecuteState execDelegate;

            //実行関数
            public virtual void Execute()
            {
                if (execDelegate != null)
                {
                    execDelegate(); //委譲関数を実行
                }
            }

            //現在のステートをストリング型で返す(c++で純粋仮想関数みたいなやつ)
            public abstract string GetStateName();
        }

        // 以下状態クラス

        //  デフォルト状態
        public class StateDefault : PlayerState
        {
            public override string GetStateName()
            {
                return "Player is Default";
            }
        }

        //  歩き状態
        public class StateWalk : PlayerState
        {
            public override string GetStateName()
            {
                return "Plaer Is Walk";
            }
        }

        //ジャンプ状態のクラス
        public class StateJump : PlayerState
        {
            public override string GetStateName()
            {
                return "Player Is Jump";
            }
        }

        //ダッシュ状態のクラス
        public class StateDash :PlayerState
        {
            public override string GetStateName()
            {
                return "Player Is Dash";
            }
        }

        //叩き状態のクラス
        public class StateStrike : PlayerState
        {
            public override string GetStateName()
            {
                return "Player Is Strike";
            }
        }

        public class StateConfusion : PlayerState
        {
            public override string GetStateName()
            {
                return "Player Is Confusion";
            }
        }

        //プレイヤーが穴におちた状態
        public class StateHoal : PlayerState
        {
            public override string GetStateName()
            {
                return "Player Is Hoal";
            }
        }

        //プレイヤーが転ぶ状態
        public class StateFall : PlayerState
        {
            public override string GetStateName()
            {
                return "Player Is Fall";
            }
        }

        //プレイヤーゲームオーバー状態
        public class StateGameOver : PlayerState
        {
            public override string GetStateName()
            {
                return "Player Is GameOver";
            }
        }
        //プレイヤーゴール状態
        public class StateGoal : PlayerState
        {
            public override string GetStateName()
            {
                return "Player Is Goal";
            }
        }

    }
}