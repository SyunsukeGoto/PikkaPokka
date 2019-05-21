using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Makoto
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField, Header("移動速度")]
        private float _speed;

        [SerializeField, Header("プレイヤー")]
        public GameObject _player;

        [SerializeField, Header("光のやつ")]
        public Goto.StarMove _starMove;

        [SerializeField, Header("ナビメッシュエージェント")]
        private NavMeshAgent _nma;

        // 移動フラグ
        private bool _moveFlag;

        // 逃げるフラグ
        private bool _escapeFlag = false;

        // 位置記憶配列
        [SerializeField]
        private Vector3[] _posMemory = new Vector3[10];

        // 振り向き速度
        private float TURN_SPEED = 2;

        private float TIME_OUT = 1.0f;
        private float _timeElapsed;

        private void Awake()
        {
            _nma.enabled = false;

        }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("誕生!" + this.transform.position);
            _nma.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            // 移動速度を適用
            _nma.speed = _speed;

            // 明かりがついていたら移動フラグを立てる
            _moveFlag = !_starMove.GetStarFlag().IsFlag((uint)Goto.StarMove.StarFlag.GENERATE_STATE);

            if (_moveFlag)
            {
                // プレイヤーの位置をセット
                _nma.SetDestination(_player.transform.position);

                // 位置を記憶する
                AddPosMemory(transform.position);

                _escapeFlag = false;
            }
            else
            {
                Vector3 v = _player.transform.position - transform.position;
                v.y = 0;

                // 光の範囲かどうか
                if (v.magnitude < _starMove.GetLightRangeRadius())
                {
                    // 10秒前の位置に向かって逃走
                    _nma.SetDestination(_posMemory[2]);

                    // 逃走フラグを立てる
                    _escapeFlag = true;
                }
                else
                {
                    if (!_escapeFlag)
                    {
                        // 止める
                        _nma.SetDestination(transform.position);

                        // プレイヤーの反対方向を向く
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-v), Time.deltaTime * TURN_SPEED);
                    }
                }
            }
        }

        private void AddPosMemory(Vector3 Pos)
        {
            _timeElapsed += Time.deltaTime;

            if(_timeElapsed >= TIME_OUT)
            {
                for (int i = 9; i > 0; i--)
                {
                    _posMemory[i] = _posMemory[i - 1];
                }

                _posMemory[0] = Pos;

                _timeElapsed = 0.0f;
            }
        }
    }
}
