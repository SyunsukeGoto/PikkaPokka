using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DistanceIndicate : MonoBehaviour
{
    private enum Mode
    {
        DirectDistance,
        MovingDistance,
    }

    [SerializeField, Header("スライダー")]
    private Slider _slider;

    [SerializeField, Header("プレイヤー")]
    private GameObject _player;

    [SerializeField, Header("スタート")]
    private Vector3 _startPos;

    public Vector3 StartPos
    {
        set
        {
            _startPos = value;
            _startDirectDistance = (_goalPos - _startPos).magnitude;
        }
    }

    [SerializeField, Header("ゴール")]
    private Vector3 _goalPos;

    public Vector3 GoalPos
    {
        set
        {
            _goalPos = value;
            _startDirectDistance = (_goalPos - _startPos).magnitude;
        }
    }

    [SerializeField, Header("モード")]
    private Mode _mode;

    [SerializeField]
    private Command _command;

    // 最初の直線距離
    private float _startDirectDistance;

    // 最初の移動距離
    private float _startMovingDistance;

    // 現在の直線距離
    private float _currentDirectDistance;

    // 現在の移動距離
    private float _currentMovingDistance;

    private NavMeshPath _path;

    private Vector3[] _temporaryPositions = new Vector3[1000];
    private Vector3[] _truePositions;

    private void Awake()
    {
        _path = new NavMeshPath();
    }

    // Start is called before the first frame update
    void Start()
    {
        _command.SetAction(Navi);
    }

    // Update is called once per frame
    void Update()
    {
        switch(_mode)
        {
            case Mode.DirectDistance:
                DirectDistance();
                break;
            case Mode.MovingDistance:
                MovingDistance();
                break;
        }
    }

    private void DirectDistance()
    {
        // 直線距離を求める
        _startDirectDistance = (_goalPos - _startPos).magnitude;

        _currentDirectDistance = (_goalPos - _player.transform.position).magnitude;

        _slider.value = _slider.maxValue - _currentDirectDistance / _startDirectDistance;

        //Debug.Log(_currentDirectDistance);
        //Debug.Log(_startDirectDistance);
        //Debug.Log((_currentDirectDistance / _startDirectDistance));
    }

    private void MovingDistance()
    {
        // 移動距離を求める
        _startMovingDistance = GetMovingDistance(_startPos, _goalPos);

        _currentMovingDistance = GetMovingDistance(_player.transform.position, _goalPos);

        _slider.value = _slider.maxValue - _currentMovingDistance / _startMovingDistance;

        Debug.Log(_startMovingDistance);
        Debug.Log(_currentMovingDistance);
    }

    private float GetMovingDistance(Vector3 startPos, Vector3 goalPos)
    {
        float dis = 0f;

        NavMesh.CalculatePath(startPos, goalPos, NavMesh.AllAreas, _path);
        _path.GetCornersNonAlloc(_temporaryPositions);
        _truePositions = _path.corners;

        Vector3 corner = startPos;

        for (int i = 0; i < _truePositions.Length; i++)
        {
            Vector3 corner2 = _truePositions[i];
            dis += Vector3.Distance(corner, corner2);
            corner = corner2;
        }

        return dis;
    }

    private void Navi()
    {
        _mode = Mode.MovingDistance;
    }
}
