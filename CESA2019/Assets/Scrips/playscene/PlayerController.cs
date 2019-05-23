﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/////////////
using Momoya;
using Momoya.PlayerState;

namespace Momoya
{
    public class PlayerController : MonoBehaviour
    {
        //構造隊の宣言
        

        //列挙型の宣言
        enum MoveDirection
        {
            UP,         //上
            DOWN,       //下
            LEFT,       //左
            RIGHT,      //右
            NONE,       //なし
            NUM,
        }

        public enum HammerState
        {
            NONE,           // なし
            WEAK,           // 弱
            NOMAL,          // 中
            STRENGTH,       // 強
        }

        enum FloorType
        {
            Normal, //普通
            Grass,  //草むら
            Swamp,  //沼地
            Gravelroad,//砂利道

            Num
        }

        //定数の定義
        public Goto.StarMove _starMove;
        bool _starFlag;//星が出ているか確認するフラグ

        [SerializeField]
        const float MoveSpeed = 5.0f; //動くスピード

        const float Speedlimit = 3.0f;
        const float DropdownPoint = -5.0f; //落下ポイント
        //変数の宣言
        private Vector3 _startPos;    //初期位置
        private float _environmentSpeed;//環境速度
        [SerializeField]
        private float[] _floorType = new float[(int)FloorType.Num]; //床の種類
        [SerializeField]
        private float _speedMagnification = 1.5f; //速度の倍率
        private float _dashSpeed;     //ダッシュスピード
        private float _nowSpeed;      //現在のスピード
        private Vector3 _vec;         //速度
        private float _nowJumpPower;  //現在のジャンプパワー
        [SerializeField]
        private float _normalJumpPower; //ノーマルジャンプ

        private Rigidbody _rg;        //リジットボディ

        [SerializeField]
        private KeyCode[] _moveKey = new KeyCode[(int)MoveDirection.NUM];//移動キー
        [SerializeField]
        private KeyCode _dashKey;                                        //ダッシュキー    
        [SerializeField]
        private KeyCode _jumpKey;                                        //ジャンプキー
        [SerializeField]
        private KeyCode _strikeKey;                                      //ハンマーで殴るキー

        private string _beforeStateName;                                 //変更前のステート名
        public StateProcessor _stateProcessor = new StateProcessor();    //プロセッサー
        //ハンマーに必要な変数
        [SerializeField]
        private int _hammerLevelLimit;                                      //ハンマーリミットレベル
        private int  _hammerLevel;                                          //ハンマーレベル
        [SerializeField]
        private float _hammerPowerLimit;                                 //ハンマーパワーリミット
        private float _hammerPower;                                      //ハンマーパワー
        [SerializeField]
        private float _hammerChargSpeed = 1.0f;                          //ハンマーチャージスピード
                                                                         //ハンマーリミット ÷ ハンマーレベル のあたい

        private float _dizzyValue = MoveSpeed / 2;                       //ふらふら度合

        private float _time;
        int _importantPoint;
        int _nowHammerState;
        int _decisionHammerState;       // キーが離され決定されたハンマー状態

        private bool _flag;             //ジャンプ  
        [SerializeField]
        Makoto.PlayerAnime _anime; //アニメーター

        bool _fallFlag;   //転ぶフラグ
        bool _fallCheckFlag;//転べるか確認するフラグ
        [SerializeField]
        float _fallCheckCount = 0.3f;//転び確認カウント
        float _fallCheckTimer;//転び確認タイマー      
        [SerializeField]
        float _fallCount = 1.0f; //転びカウント
        float _fallTimer;        //転びタイマー

        [SerializeField]
        int _smallHolecount = 10; //小穴カウント
        [SerializeField]
        int _mediumHoleCount = 20;//中穴カウント
        [SerializeField]
        int _bigHoleCount;   //大穴カウント
        int _goalRevaGachaCount;//目標のレバガチャカウント
        int _nowRevaGachaCount; //現在のレバガチャカウント
        private MoveDirection _revaGachaState;//レバガチャ状態
        private MoveDirection _currentRevaGachaState;//1フレーム前のレバガチャ状態
        private bool _holeFlag;//穴落ちフラグ

        bool _rotationFlag; //回転フラグ
        bool _strikeMode;   //たたく状態
        public GameObject crushableBox; //壊せる箱
        Camera _camera;


        //ステートの宣言
        public StateDefault _stateDefault = new StateDefault();                 //デフォルト状態
        public StateWalk _stateWalk = new StateWalk();                          //歩き状態
        public StateJump _stateJump = new StateJump();                          //ジャンプ状態
        public StateDash _stateDash = new StateDash();                          //ダッシュ状態
        public StateStrike _stateStrike = new StateStrike();                    //叩く状態
        public StateConfusion _stateConfusion = new StateConfusion();           //混乱状態
        public StateHoal _stateHoal = new StateHoal();                          //穴状態
        public StateFall _stateFall = new StateFall();                          //転び状態
        public StateGameOver _stateGameOver = new StateGameOver();              //ゲームオーバー状態
        public StateGoal _stateGoal = new StateGoal();                          //ゴール状態
        public StateBreakBox _stateBreakBox = new StateBreakBox();              //箱を壊す状態
        //////////デバッグ用
        //////////デバッグ用
        public Text _chargeText;     //現在のパワーを表示するデバッグ用変数
        public Text _levelText;      //現在のレベルを表示するデバッグ用変数
        private float _playerAngle ;

        // Use this for initialization
        void Start()
        {

            _playerAngle = -180;
            Debug.Log(_playerAngle);
            //プレイヤーの初期設定
            _rg = GetComponent<Rigidbody>(); //リジットボディの取得
            _startPos = _rg.position;        //初期位置の設定
            _dashSpeed = MoveSpeed * _speedMagnification;
            _nowSpeed = MoveSpeed;
            _nowJumpPower = _normalJumpPower;
            _time = 0f;


            if (_hammerLevelLimit <= 0)
            {
                _hammerLevelLimit = 1; //0にはしない
            }

            _hammerLevel = 0;                //ハンマーのレベル
            _hammerPower = 0.0f;             //ハンマーのパワー
            _importantPoint = (int)_hammerPowerLimit / _hammerLevelLimit;
            _nowHammerState = (int)HammerState.NONE;
            _decisionHammerState = (int)HammerState.NONE;

            _fallFlag = false;
            _fallCheckFlag = false;
            _fallCheckTimer = 0.0f;
            _fallTimer = 0.0f;
            _goalRevaGachaCount = 0;
            _nowRevaGachaCount = 0;
            _rotationFlag = false;
            _strikeMode = false;
            _camera = Camera.main; //メインのカメラを取得

            //初期ステートをdefaultにする
            _stateProcessor.State = _stateDefault;
            //委譲の設定
            _stateDefault.execDelegate = Default;
            _stateWalk.execDelegate = Walk;
            _stateJump.execDelegate = Jump;
            _stateDash.execDelegate = Dash;
            _stateStrike.execDelegate = Strike;
            _stateBreakBox.execDelegate = BreakBox;
            _stateConfusion.execDelegate = Confusion;
            _stateHoal.execDelegate = Hoal;
            _stateFall.execDelegate = Fall;
            _stateGameOver.execDelegate = GameOver;
            _stateGoal.execDelegate = StageGoal;
        }

        // Update is called once per frame
        void Update()
        {
            PlayerCtrl();
            //DebugCtrl(); //デバッグ用

            //Debug.Log(_nowHammerState.ToString());
            Debug.Log(_decisionHammerState.ToString());
            if (_nowHammerState == (int)HammerState.NONE)
            {
                _decisionHammerState = (int)HammerState.NONE;
            }
            
            //ステートの値が変更されたら実行処理を行う
            if (_stateProcessor.State == null)
            {
                return;
            }

            //現在どのステートか確認するためのデバッグ処理
            if (_stateProcessor.State.GetStateName() != _beforeStateName)
            {
                //Debug.Log(" Now State:" + _stateProcessor.State.GetStateName());
                _beforeStateName = _stateProcessor.State.GetStateName();

            }

            _stateProcessor.Execute();//実行関数
        }

        public bool FallCheck()
        {
            //転べる状態の時尚且つプレイヤーが移動しているときにカウントを進める
            if (_fallCheckFlag == true && ((Mathf.Abs(Input.GetAxis("Vertical")) >= 0.3f) || (Mathf.Abs(Input.GetAxis("Horizontal")) >= 0.3f)))
            {
                _fallCheckTimer += Time.deltaTime;
                
                if (_fallCheckTimer > _fallCheckCount)
                {
                    int fallNum = UnityEngine.Random.Range(0, 100);
                    int trueNum = 90;

                    if (fallNum >= trueNum)
                    {

                        _fallFlag = true;
                    }
                    _fallCheckTimer = 0.0f;//転び確認タイマーをもとに戻す
                }

            }
            return true;
        }

        //移動関数
        public void Move()
        {
            float angle = 2.0f;
            Debug.Log("_vec = _camera.transform.forward * _nowSpeed; = " + _camera.transform.forward * _nowSpeed);
            if (Input.GetKey(_moveKey[(int)MoveDirection.UP]))
            {

                _vec = _camera.transform.forward * _nowSpeed;
                
                //_vec.z = _nowSpeed;
            }


            if (Input.GetKey(_moveKey[(int)MoveDirection.DOWN]))
            {
                _vec = -_camera.transform.forward * _nowSpeed;
                //_vec.z = -_nowSpeed;
            }

            if (Input.GetKey(_moveKey[(int)MoveDirection.RIGHT]))
            {
                _playerAngle += angle;
                //_vec.x = _nowSpeed;
            }

            if (Input.GetKey(_moveKey[(int)MoveDirection.LEFT]))
            {
                _playerAngle -= angle;
                // _vec.x = -_nowSpeed;
            }
            /////////ここより下は停止用処理

            //if (Input.GetKeyUp(_moveKey[(int)MoveDirection.UP]))
            //{
            //    _vec.z *= 0.0f;
            //}

            //if (Input.GetKeyUp(_moveKey[(int)MoveDirection.DOWN]))
            //{
            //    _vec.z *= 0.0f;
            //}

            //if (Input.GetKeyUp(_moveKey[(int)MoveDirection.RIGHT]))
            //{
            //    _vec.x *= 0.0f;
            //}

            //if (Input.GetKeyUp(_moveKey[(int)MoveDirection.LEFT]))
            //{
            //    _vec.x *= 0.0f;
            //}
            _vec.x *= 0.95f;
            _vec.z *= 0.95f;
            _rotationFlag = true;
            _anime.Walk(); //歩かせる
        }

        public void ConfusionMove(int confusionValue)
        {
            if (Input.GetKey(_moveKey[(int)MoveDirection.UP]))
            {
                _vec.z = _nowSpeed;

                if (Mathf.PerlinNoise(Time.time * 5, 0) < 0.5)
                {
                    _vec.x = _dizzyValue * confusionValue;
                }
                else if (Mathf.PerlinNoise(Time.time * 5, 0) > 0.5)
                {
                    _vec.x = -_dizzyValue * confusionValue;
                }
                else
                {
                    _vec.x = 0;
                }
            }

            if (Input.GetKey(_moveKey[(int)MoveDirection.DOWN]))
            {
                _vec.z = -_nowSpeed;

                if (Mathf.PerlinNoise(Time.time * 5, 0) < 0.5)
                {
                    _vec.x = -_dizzyValue * confusionValue;
                }
                else if (Mathf.PerlinNoise(Time.time * 5, 0) > 0.5)
                {
                    _vec.x = _dizzyValue * confusionValue;
                }
                else
                {
                    _vec.x = 0;
                }
            }

            if (Input.GetKey(_moveKey[(int)MoveDirection.RIGHT]))
            {
                _vec.x = _nowSpeed;

                if (Mathf.PerlinNoise(Time.time * 5, 0) < 0.5)
                {
                    _vec.z = _dizzyValue * confusionValue;
                }
                else if (Mathf.PerlinNoise(Time.time * 5, 0) > 0.5)
                {
                    _vec.z = -_dizzyValue * confusionValue;
                }
                else
                {
                    _vec.z = 0;
                }
            }

            if (Input.GetKey(_moveKey[(int)MoveDirection.LEFT]))
            {
                _vec.x = -_nowSpeed;

                if (Mathf.PerlinNoise(Time.time * 5, 0) < 0.5)
                {
                    _vec.z = -_dizzyValue * confusionValue;
                }
                else if(Mathf.PerlinNoise(Time.time * 5, 0) > 0.5)
                {
                    _vec.z = _dizzyValue * confusionValue;
                }
                else
                {
                    _vec.z = 0;
                }
            }

            if (Input.GetKeyUp(_moveKey[(int)MoveDirection.UP]))
            {
                _vec.z = 0.0f;
                _vec.x = 0.0f;
            }

            if (Input.GetKeyUp(_moveKey[(int)MoveDirection.DOWN]))
            {
                _vec.z = 0.0f;
                _vec.x = 0.0f;
            }

            if (Input.GetKeyUp(_moveKey[(int)MoveDirection.RIGHT]))
            {
                _vec.x = 0.0f;
                _vec.z = 0.0f;
            }

            if (Input.GetKeyUp(_moveKey[(int)MoveDirection.LEFT]))
            {
                _vec.x = 0.0f;
                _vec.z = 0.0f;
            }
        }

        //ハンマーパワーをchargeする関数
        void ChargeHammerPower()
        {
            //ハンアーキーを押されたら
            if (Input.GetKey(_strikeKey))
            {
                _hammerPower += Time.deltaTime * _hammerChargSpeed;
            }
            //Debug.Log(_hammerPower.ToString());
            //ハンマーパワーを上限を越させない
            if(_hammerPower > _hammerPowerLimit)
            {
                _hammerPower = _hammerPowerLimit;
            }

            if(_strikeMode == false)
            {
                if (_hammerPower > 5)
                    _nowHammerState = (int)HammerState.WEAK;
                if (_hammerPower > 10)
                    _nowHammerState = (int)HammerState.NOMAL;
                if (_hammerPower > 15)
                    _nowHammerState = (int)HammerState.STRENGTH;
            }
            else
            {
                _nowHammerState = (int)HammerState.NONE;
            }

  
           
            

        }
        //ハンマーレベルをチェックする関数
        int LevelCheck(int importantPoint, int power)
        {
            int rLevel = 0;
            
            while(true)
            {
                if (power >= importantPoint)
                {
                    rLevel++;
                    power -= importantPoint;
                }
                else
                {
                    //もし0なら1を返す
                    if(rLevel == 0)
                    {
                        rLevel = 1;
                    }
                  break;
                }
           }
            //アニメーター変更
            switch (rLevel)
            {
                case (int)HammerState.WEAK: _anime.WeakAttack(); break;
                case (int)HammerState.NOMAL: _anime.NomalAttack(); break;
                case (int)HammerState.STRENGTH: _anime.StrengthAttack(); break;
            }


            return rLevel; 
        }

        /// <summary>
        /// レバガチャ関数
        /// </summary>
        /// <returns>ガチャガチャしてたらtrueを返す</returns>ガチャガチャしてなかったらfalseを返す
        public bool RevaGacha()
        {
            _currentRevaGachaState = _revaGachaState;

            if (Input.GetAxis("Vertical") >= 0.3f)
            {
                _revaGachaState = MoveDirection.UP;
            }

            if (Input.GetAxis("Vertical") <= -0.3f)
            {
                _revaGachaState = MoveDirection.DOWN;
            }

            if (Input.GetAxis("Horizontal") >= 3.0f)
            {
                _revaGachaState = MoveDirection.RIGHT;
            }

            if (Input.GetAxis("Horizontal") <= -3.0f)
            {
                _revaGachaState = MoveDirection.LEFT;
            }

            if (Mathf.Abs(Input.GetAxis("Vertical")) <= 0.3f && Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.3f)
            {
                _revaGachaState = MoveDirection.NONE;
            }

            //前の方向と違う&何か知らおしていたらtrue
            if (_currentRevaGachaState != _revaGachaState)
            {
                return true;
            }
            return false;
        }
        public void PlayerCtrl()
        {
            FallCheck();
            //転びflagがtrueなら転び状態へ
            if (_fallFlag == true)
            {
                _stateProcessor.State = _stateFall;
            }
            //穴落ちがtrueなら穴落ち状態へ
            if (_holeFlag == true)
            {
                _stateProcessor.State = _stateHoal;
            }

            //速度を足す
            _rg.velocity = new Vector3(_vec.x * _environmentSpeed, _rg.velocity.y, _vec.z * _environmentSpeed);

            //落下ポイントよりポジションが低ければ初期位置に戻す
            if (_rg.position.y < DropdownPoint)
            {
                _rg.position = _startPos;
            }

            //ゲーコン確認用
            //_rotationFlag = true;

            if(_rotationFlag)
            {
                //プレイヤーの角度を取得
                //_playerAngle = Mathf.Lerp(_playerAngle, SetAngle(),0.2f);
                //_playerAngle = SetAngle();
                transform.localRotation = Quaternion.Euler(0, _playerAngle, 0);
            }

           
            //// カメラの方向から、X-Z平面の単位ベクトルを取得
            //Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            //// 方向キーの入力値とカメラの向きから、移動方向を決定
            //Vector3 moveForward = cameraForward * Input.GetAxis("Vertical") + _camera.transform.right * Input.GetAxis("Horizontal");
            //_vec.y = 0;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_vec), Time.deltaTime * 1.2f);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_camera.transform.rotation.y *  _vec), 1.0f);
            //transform.position += _camera.transform.rotation.y * _vec;
            ////プレイヤーの角度を回転に渡す
            //
            //transform.localRotation = Quaternion.Euler(0, _playerAngle, 0);
            // キャラクターの向きを進行方向に

        }

        //止まっているかチェックする関数
        public bool CheckStop()
        {
          //  Debug.Log(_vec);
            Vector2 tmp = new Vector2(_vec.x, _vec.z);
            //止まっていたらtrueを返す
            if (Mathf.Abs(tmp.magnitude) <= 0.1f)
            {
                return true;
            }
            //止まっていないことを伝える
            return false;
        }
        /////////ステートの関数をここに記入

        //通常状態
        public void Default()
        {
            if(Input.GetKeyDown(_dashKey))
            {
                _stateProcessor.State = _stateDash;
            }

            //ジャンプキーを押されたらジャンプ状態へ
            if (Input.GetKeyDown(_jumpKey) && _flag)
            {
                _stateProcessor.State = _stateJump;
            }


            //ジャンプキーを押されたらハンマー状態へ
            if (Input.GetKeyDown(_strikeKey))
            {
                _stateProcessor.State = _stateStrike;
            }

            //移動キーのどれかが押されたら移動状態に切り替える
            for (int i = 0; i < (int)MoveDirection.NUM - 1; i++)
            {
                if(Input.GetKey(_moveKey[i]))
                {
                    _stateProcessor.State = _stateWalk;
                }
                           
            }
            _rotationFlag = false;
            _anime.Idle();
        }

        //歩き状態の関数
        public void Walk()
        {
            //速度をムーブスピードにする
            _nowSpeed = MoveSpeed;
            //移動する
           
            Move();
           // ConfusionMove(1);

            //ダッシュキーを押されたら走るステートに切り替え
            if(Input.GetKey(_dashKey))
            {
                _stateProcessor.State = _stateDash;
            }

            //ジャンプキーを押されたらジャンプ状態へ
            if(Input.GetKeyDown(_jumpKey) && _flag)
            {
                _stateProcessor.State = _stateJump;
            }

            //ジャンプキーを押されたらハンマー状態へ
            if (Input.GetKeyDown(_strikeKey))
            {
                _stateProcessor.State = _stateStrike;
            }

            //止まっていたらステートを通常状態にする
            if (CheckStop() == true)
            {
                _stateProcessor.State = _stateDefault;
            }
        }
        
        //ジャンプ状態
        public void Jump()
        {
           // _rg.AddForce(Vector3.up * _nowJumpPower);
            //ジャンプ後defaultに戻す
            _stateProcessor.State = _stateDefault;
        }

        //ダッシュ状態
        public void Dash()
        {
            _nowSpeed = _dashSpeed;
            //移動する
            Move();
            
            ////ダッシュキーを押されたら走るステートに切り替え
            if (Input.GetKeyUp(_dashKey))
            {
                _stateProcessor.State = _stateWalk;
            }

            //ジャンプキーを押されたらジャンプ状態へ
            if (Input.GetKeyDown(_jumpKey) && _flag)
            {
                _stateProcessor.State = _stateJump;
            }

            //ジャンプキーを押されたらハンマー状態へ
            if (Input.GetKeyDown(_strikeKey))
            {
                _stateProcessor.State = _stateStrike;
            }

            //止まっていたらステートを通常状態にする
            if (CheckStop() == true)
            {
                _stateProcessor.State = _stateDefault;
            }

        }

        //叩き状態
        public void Strike()
        {
            _dashSpeed = Speedlimit;
           // Move();//歩く

            //ハンマーパワーをチャージ
            ChargeHammerPower();

            //ハンマーキーを離したら
            if (Input.GetKeyUp(_strikeKey))
            {
                _hammerLevel = LevelCheck( _importantPoint, (int)_hammerPower);
                //パワーを0にする
                _hammerPower = 0.0f;
                _decisionHammerState = _nowHammerState;
                _nowHammerState = (int)HammerState.NONE;
                //たたき状態フラグがfalseならふらふら状態へ
                if (_strikeMode == false)
                {
                   _stateProcessor.State = _stateConfusion;
                }
                else
                {
                    //trueなら箱を壊すステートへ
                    _stateProcessor.State = _stateBreakBox;
                }

                

                //_stateProcessor.State = _stateDefault;
            }

            //if (_decisionHammerState != (int)HammerState.NONE)
            //{
            //    _time++;
            //    _decisionHammerState = _time < 90 ? (int)HammerState.NONE : _decisionHammerState;
            //    _time = _time < 90 ? 0f : _time;
            //}

        }
        //箱を壊す
        public void BreakBox()
        {
                crushableBox.GetComponent<CrushableBox>().DethCall(10);
                _strikeMode = false;
                //デフォルト状態へ  
                _stateProcessor.State = _stateDefault;
        }

        public void Confusion()
        {
            //速度をムーブスピードにする
            _nowSpeed = MoveSpeed;
            //移動する
            ConfusionMove(1);

            if (_starMove.GetStarFlag().IsFlag((uint)Goto.StarMove.StarFlag.GENERATE_STATE) == false)
            {
                //ダッシュキーを押されたら走るステートに切り替え
                if (Input.GetKey(_dashKey))
                {
                    _stateProcessor.State = _stateDash;
                }

                //ジャンプキーを押されたらジャンプ状態へ
                if (Input.GetKeyDown(_jumpKey) && _flag)
                {
                    _stateProcessor.State = _stateJump;
                }

                //ハンマキーーを押されたらハンマー状態へ
                if (Input.GetKeyDown(_strikeKey))
                {
                    _stateProcessor.State = _stateStrike;
                }

                //止まっていたらステートを通常状態にする
                if (CheckStop() == true)
                {
                    _stateProcessor.State = _stateDefault;
                }

                ////移動キーのどれかが押されたら移動状態に切り替える
                //for (int i = 0; i < (int)MoveDirection.NUM - 1; i++)
                //{
                //    if (Input.GetKey(_moveKey[i]))
                //    {
                //        _vec = Vector3.zero;
                //        _stateProcessor.State = _stateWalk;
                //    }

                //}
            }
           // _rotationFlag = true;
        }

        public int GetHammerState()
        {
            return _decisionHammerState;
        }

        public int GetNowHammerState()
        {
            return _nowHammerState;
        }

        //穴に落ちた状態
        public void Hoal()
        {
            _vec = Vector3.zero; //速度を0にする

            bool revaGachaFlag = RevaGacha();
            if (revaGachaFlag)
            {
                _nowRevaGachaCount++;
            }

            if (_nowRevaGachaCount > _goalRevaGachaCount)
            {
                _nowRevaGachaCount = 0;
                _holeFlag = false;
                _stateProcessor.State = _stateDefault;
            }
        }

        /// <summary>
        /// 転び状態
        /// 一定時間たったらdefaultに戻す
        /// </summary>
        public void Fall()
        {
            _fallCheckFlag = false;
            _fallFlag = false;
            _vec = Vector3.zero;//速度を0
            _fallTimer += Time.deltaTime;

            if (_fallTimer > _fallCount)
            {
                _fallTimer = 0.0f; //タイマーを0に戻す
                _stateProcessor.State = _stateDefault;
            }
        }

        /// <summary>
        /// ゲームオーバーの関数
        /// </summary>
        public void GameOver()
        {

        }
        /// <summary>
        /// ゴールの関数
        /// </summary>
        public void StageGoal()
        {

        }

        public float SetAngle()
        {

            float angle ;

            return angle = (float)Math.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * UnityEngine.Mathf.Rad2Deg;
        }

        //デバッグ用関数
        public void DebugCtrl()
        {
            _chargeText.text = _hammerPower.ToString(); //現在のハンマーパワー
            _levelText.text = _hammerLevel.ToString();  //現在のレベル
        }

        //当たり判定(stay)床に対しての効果
        public void OnCollisionStay(Collision collision)
        {
            switch (collision.transform.tag)
            {
                case "Normal": _environmentSpeed = _floorType[(int)FloorType.Normal]; break; //普通の速度
                case "Grass": _environmentSpeed = _floorType[(int)FloorType.Grass]; break; //草むらの速度
                case "Swamp": _environmentSpeed = _floorType[(int)FloorType.Swamp]; break; //沼地の速度
                case "Gravelroad": _environmentSpeed = _floorType[(int)FloorType.Gravelroad]; _fallCheckFlag = true; break; //砂利道の速度
                case "Goal": _stateProcessor.State = _stateGoal; break;
             //   case "CrushableBox": _strikeMode = true; crushableBox  =  collision.gameObject; Debug.Log("箱発見"); break;
            }
            if (collision.gameObject.layer == 9)
            {
                _flag = true;
            }
        }

        /// <summary>
        /// 当たり判定はなれたとき
        /// </summary>
        /// <param name="collision"></param>
        public void OnCollisionExit(Collision collision)
        {
            switch (collision.transform.tag)
            {
                case "Normal": break; //普通の速度
                case "Grass": break; //草むらの速度
                case "Swamp": break; //沼地の速度
                case "Gravelroad": _fallCheckFlag = false; break; //砂利道の速度
                case "Goal": break;
              //  case "CrushableBox": _strikeMode = false; crushableBox = null;  break;
            }
            if (collision.gameObject.layer == 9)
            {
                _flag = false;
            }
        }


        //当たり判定穴などの触れた瞬間に発動するのをチェック
        public void OnCollisionEnter(Collision collision)
        {
            switch (collision.transform.tag)
            {
                case "Hole":
                    Hole hitHole = collision.transform.GetComponent<Hole>();
                    switch (hitHole.GetType)
                    {
                        case Hole.HoleType.Small: _goalRevaGachaCount = _smallHolecount; _holeFlag = true; break;
                        case Hole.HoleType.Medium: _goalRevaGachaCount = _mediumHoleCount; _holeFlag = true; break;
                        case Hole.HoleType.Big: _goalRevaGachaCount = _bigHoleCount; _stateProcessor.State = _stateGameOver; break;
                    }

                    Destroy(collision.gameObject);//穴を消す
                    break;
            }
        

        }
        //たたき状態を分けるプロパティ
        public bool StrikeMode
        {
            get { return _strikeMode; }
            set { _strikeMode = value; }
        }
        //たたき壊せる箱のプロパティ
        public GameObject CrushableBox
        {
            get { return crushableBox; }
            set { crushableBox = value; }
        }

        public float Angle
        {
           get { return _playerAngle; }
          
        }

    }
}