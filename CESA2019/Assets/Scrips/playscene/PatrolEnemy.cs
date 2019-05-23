using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Chase,
    }

    [SerializeField, Range(0.0f, 10.0f),Header("移動速度")]
    private float _speed;

    [SerializeField, Header("プレイヤー")]
    private GameObject _player;

    [SerializeField, Range(0.0f, 10.0f), Header("索敵範囲")]
    private float _searchRange;

    [SerializeField, Header("ナビメッシュエージェント")]
    private NavMeshAgent _nma;

    [SerializeField, Header("巡回ポイント")]
    private GameObject[] _patrolPoint;
    
    [SerializeField, Header("追跡するか")]
    private bool _isChase;

    [SerializeField, Header("ダメージを受けるか")]
    private bool _isDamaged;

    // 現在向かっている巡回ポイントのナンバー
    private int _currentPoint;

    // 現在のステート
    private State _currentState;

    // Start is called before the first frame update
    void Start()
    {
        _nma.speed = _speed;

        _currentPoint = 0;

        _currentState = State.Patrol;

        _nma.SetDestination(_patrolPoint[_currentPoint].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
        }
    }

    private void Patrol()
    {
        Vector3 v = _patrolPoint[_currentPoint].transform.position - transform.position;
        v.y = 0;

        if (v.magnitude < 0.1f)
        {
            if(_currentPoint + 1 >= _patrolPoint.Length)
            {
                _currentPoint = 0;
            }
            else
            {
                _currentPoint++;
            }

            _nma.SetDestination(_patrolPoint[_currentPoint].transform.position);
        }

        if(_isChase)
        {
            v = _player.transform.position - transform.position;

            if(v.magnitude <= _searchRange)
            {
                _nma.SetDestination(transform.position);

                _currentState = State.Chase;
            }
        }
    }

    private void Chase()
    {
        _nma.SetDestination(_player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isDamaged)
        {

        }
    }
}
