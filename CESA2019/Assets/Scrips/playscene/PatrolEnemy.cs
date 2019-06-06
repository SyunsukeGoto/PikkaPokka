using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Makoto
{
    public class PatrolEnemy : MonoBehaviour
    {
        public enum State
        {
            Patrol, // 巡回
            Chase,  // 追跡
            Death,  // 死亡
        }

        [SerializeField, Range(0.0f, 100.0f), Header("移動速度")]
        private float _speed;

        [SerializeField, Header("プレイヤー")]
        public GameObject _player;

        [SerializeField, Header("光のやつ")]
        public Goto.StarMove _starMove;

        Goto.StarMove StarMove
        {
            set { _starMove = value; }
        }

        [SerializeField, Range(0.0f, 100.0f), Header("視界の距離")]
        private float _searchRange;

        [SerializeField, Range(0.0f, 360.0f), Header("視界の範囲")]
        private float _searchAngle;

        [SerializeField, Header("ナビメッシュエージェント")]
        public NavMeshAgent _nma;

        [SerializeField, Header("巡回ポイント")]
        public GameObject[] _patrolPointA;

        [SerializeField, Header("巡回ポイント")]
        public GameObject[] _patrolPointB;

        [SerializeField, Header("追跡するか")]
        private bool _isChase;

        [SerializeField, Header("攻撃できるか")]
        private bool _isAttack;

        [SerializeField, Header("プレイヤーに反撃するか")]
        private bool _isCounterAttack;

        [SerializeField, Header("精子")]
        private bool _active;

        [SerializeField, Header("追跡中に光ったら巡回状態に戻る")]
        private bool _penis;

        [SerializeField, Range(0.0f, 10.0f), Header("巡回状態に戻るまでの時間")]
        private float _stateReturnTime;

        [SerializeField, Header("アニメーター")]
        private Animator _anime;

        [SerializeField, Header("コライダー")]
        public CapsuleCollider[] _col = new CapsuleCollider[2];

        private bool _bFlag;

        private bool _moveFlag = false;
        public bool Active
        {
            set { _active = value; }
        }

        public bool IsAttack
        {
            get { return _isAttack; }
        }

        // 現在向かっている巡回ポイントのナンバー
        private int _currentPoint;

        // 現在のステート
        private State _currentState;

        private float _time;

        public State CurrentState
        {
            get { return _currentState; }
        }


        // Start is called before the first frame update
        void Start()
        {
            _nma.enabled = false;
            _currentPoint = 0;

            _currentState = State.Patrol;

            _active = true;

            for(int i = 0; i< _patrolPointA.Length;i++)
            {
                //Debug.Log("入ったやつ" +_patrolPointA[i].transform.position);
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (_moveFlag == false)
            {
                _nma.enabled = true;
            }
            
            if(_time > 2.0f && _moveFlag == false)
            {
                _moveFlag = true;
                _time = 0;
            }

            _nma.speed = _speed;
            if (_nma.pathStatus != NavMeshPathStatus.PathInvalid)
            {
                if (_active)
                {
                    switch (_currentState)
                    {
                        case State.Patrol:
                            Patrol();
                            //Debug.Log("Patrol");
                            break;
                        case State.Chase:
                            Chase();
                            //Debug.Log("Chase");
                            break;
                        case State.Death:
                            Death();
                            break;
                    }
                }
                else
                {
                    _nma.SetDestination(transform.position);
                }
            }
        }

        // 巡回
        private void Patrol()
        {
                Vector3 v;
                if (!_bFlag)
                {
                    _nma.SetDestination(_patrolPointA[_currentPoint].transform.position);

                    v = _patrolPointA[_currentPoint].transform.position - transform.position;
                    v.y = 0;

                    if (v.magnitude < 0.1f)
                    {
                        if (_currentPoint + 1 >= _patrolPointA.Length)
                        {
                            _currentPoint = 0;
                        }
                        else
                        {
                            _currentPoint++;
                        }
                    }
                }
                else
                {
                    _nma.SetDestination(_patrolPointB[_currentPoint].transform.position);

                    v = _patrolPointB[_currentPoint].transform.position - transform.position;
                    v.y = 0;

                    if (v.magnitude < 0.1f)
                    {
                        if (_currentPoint + 1 >= _patrolPointB.Length)
                        {
                            _currentPoint = 0;
                        }
                        else
                        {
                            _currentPoint++;
                        }
                    }
                }

                if (_isChase)
                {
                    if (!_starMove.GetStarFlag().IsFlag((uint)Goto.StarMove.StarFlag.GENERATE_STATE))
                    {
                        if (IsComeInSight())
                        {
                            _nma.SetDestination(transform.position);

                            _currentState = State.Chase;
                        }
                    }
                }
            
            
        }

        // 追跡
        private void Chase()
        {
            if (!IsComeInSight())
            {
                _time += Time.deltaTime;
            }
            else
            {
                _time = 0;
            }

            if (_penis)
            {
                if (_starMove.GetStarFlag().IsFlag((uint)Goto.StarMove.StarFlag.GENERATE_STATE))
                {
                    _time += _stateReturnTime;
                }
            }

            if (_time >= _stateReturnTime)
            {
                _currentState = State.Patrol;
                _currentPoint = 0;
            }
            else
            {
                _nma.SetDestination(_player.transform.position);
            }
        }

        // 視界内にプレイヤーが存在するかを調べる
        private bool IsComeInSight()
        {
            Vector3 v = _player.transform.position - transform.position;

            if (v.magnitude <= _searchRange)
            {
                float r = (transform.rotation.eulerAngles.y + 90) * Mathf.Deg2Rad;

                Vector3 v2 = new Vector3(Mathf.Cos(r), 0, Mathf.Sin(r));

                float dot = Vector3.Dot(v.normalized, v2.normalized);

                float rad = Mathf.Acos(dot);

                if (rad < _searchAngle * Mathf.Deg2Rad)
                {
                    if (!Physics.Linecast(transform.position, _player.transform.position, LayerMask.GetMask("Obstacle")))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // 巡回ポイントをBに変更する
        public void BflagOn()
        {
            _bFlag = true;
            _currentPoint = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Attack")
            {
                Active = false;

                if (_isCounterAttack)
                {
                    // プレイヤーの体力を減らす
                    _player.GetComponent<Momoya.PlayerController>().GhostDamage();
                }
            }
        }

        public void MoveStart()
        {
            _currentPoint = Random.Range(0, _patrolPointA.Length);
            _nma.SetDestination(_patrolPointA[_currentPoint].transform.position);
        }

        public void DeathOn()
        {
            _currentState = State.Death;
            _anime.SetBool("Death", true);
            _time = 0;
            _nma.SetDestination(transform.position);
            _col[0].enabled = false;
            _col[1].enabled = false;
        }

        private void Death()
        {
            _time += Time.deltaTime;

            if(_time >= 1)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
