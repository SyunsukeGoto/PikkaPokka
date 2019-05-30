//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
/// <file>		StarMove.cs
/// 
/// <brief>		☆関連のC++
/// 
/// <date>		2019/4/14
/// 
/// <author>	後藤　駿介
//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///-----------------------------------------------------------------------------
/// 
/// </brief> StarMoveクラス
///
///-----------------------------------------------------------------------------
namespace Goto
{
    public class StarMove : MonoBehaviour
    {
        public enum StarFlag : uint
        {
            GENERATE_STATE = (1 << 0),     // 生成フラグ
        }

        public enum LightRange
        {
            NONE,
            WEAK,
            NOMAL,
            STRENGTH,
        }

        public const int STAR_LIFE = 300;

        [SerializeField]
        private GameObject _starPrefab;                             // 星のプレハブ

        private GameObject[] _starObject = new GameObject[5];       // 星のゲームオブジェクト

        private float _starAngle;                                   // 角度

        private GameObject _parent;                                 // 親になっているオブジェクト

        private float _radius;                                      // 半径の調整

        private Flag _starflag;                                     // 星のフラグ

        private float _shineRange;                                  // 輝量

        private float _time;

        private LightRange _lightRange;

        [SerializeField]
        private GameObject _playerControllerObject;
        private Momoya.PlayerController _playerController;
        private bool _finishFlag;
        /// <summary>
        /// 初期化処理
        /// </summary>
        void Start()
        {
            _starflag = new Flag();
            _starAngle = 0f;
            _radius = 0f;
            _time = 0f;
            _playerController = _playerControllerObject.GetComponent<Momoya.PlayerController>();
            _lightRange = LightRange.NONE;
            _finishFlag = false;     
        }
        
        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            if(Time.timeScale == 1)
            {
                SetStarInformation();

                StarMovement();
            }

        }

        /// <summary>
        /// 星情報のセット
        /// </summary>
        void SetStarInformation()
        {
            //ハンマー状態がたたかれてるか
            if (_playerController.GetHammerState() != (int)Momoya.PlayerController.HammerState.NONE)
            {
                // フラグが立っていなかったら
                if (!_starflag.IsFlag((uint)StarFlag.GENERATE_STATE))
                {
                    switch (_playerController.GetHammerState())
                    {
                        case (int)Momoya.PlayerController.HammerState.WEAK:
                            _shineRange = 1.5f;
                            _radius = 3.5f;
                            _lightRange = LightRange.WEAK;
                            break;

                        case (int)Momoya.PlayerController.HammerState.NOMAL:
                            _shineRange = 2.5f;
                            _radius = 2.5f;
                            _lightRange = LightRange.NOMAL;
                            break;

                        case (int)Momoya.PlayerController.HammerState.STRENGTH:
                            _shineRange = 3.5f;
                            _radius = 1.5f;
                            _lightRange = LightRange.STRENGTH;
                            break;

                        default:
                            _lightRange = LightRange.NONE;
                            break;
                    }

                    for (int i = 0; i < _starObject.Length; i++)
                    {
                        _starObject[i] = Instantiate(_starPrefab) as GameObject;
                        _starObject[i].GetComponent<Light>().range = _shineRange;
                    }
                    _parent = transform.root.gameObject;
                    _starflag.OnFlag((uint)StarFlag.GENERATE_STATE);
                }
            }
        }

        /// <summary>
        /// 星の動き
        /// </summary>
        void StarMovement()
        {
            if (_starflag.IsFlag((uint)StarFlag.GENERATE_STATE) && !_finishFlag)
            {
              
                _time++;
                if (_time > STAR_LIFE)
                {
                    _shineRange -= 0.1f;
                    if (_shineRange < 0f)
                    {
                        _shineRange = 0f;
                        _time = 0f;
                        _starflag.OffFlag((uint)StarFlag.GENERATE_STATE);
                    }
                }

                _starAngle++;
                _starAngle = _starAngle > 360 ? 0 : _starAngle;

                for (int i = 0; i < _starObject.Length; i++)
                {
                    _starObject[i].GetComponent<Rigidbody>().useGravity = false;
                    int angle = i * (360 / 5);
                    float x = Mathf.Cos((angle + _starAngle) * Mathf.Deg2Rad) / _radius;
                    float z = Mathf.Sin((angle + _starAngle) * Mathf.Deg2Rad) / _radius;
                    _starObject[i].transform.position = new Vector3(_parent.transform.position.x + x, transform.position.y, _parent.transform.position.z + z);
                    _starObject[i].GetComponent<Light>().range = _shineRange;

                    if(_shineRange <= 0f)
                    {
                        _starObject[i].GetComponent<Rigidbody>().useGravity = true;
                        _starObject[i] = null;
                        
                    }
                }
            }
        }

        public Flag GetStarFlag()
        {
            return _starflag;
        }

        public float GetLightRangeRadius()
        {
            switch(_lightRange)
            {
                case LightRange.NONE:
                    return 0.0f;
                case LightRange.WEAK:
                    return 1.0f;
                case LightRange.NOMAL:
                    return 1.5f;
                case LightRange.STRENGTH:
                    return 2.0f;
            }

            return 0.0f;
        }
    }
}