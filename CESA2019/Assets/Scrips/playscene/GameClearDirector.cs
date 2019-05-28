using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearDirector : MonoBehaviour
{
    private enum State
    {
        Star,
        FadeIn,
        Walk,
    }

    [SerializeField, Range(1, 10), Header("何秒で次のシーンに移行するか")]
    private int _timeLimit;

    [SerializeField, Header("次のシーンの名前")]
    private string _nextSceneName;

    [SerializeField, Header("カメラ")]
    private Camera _camera;

    [SerializeField, Range(1.0f,10.0f), Header("スピード")]
    private float _speed;

    [SerializeField, Header("フェードインに使うイメージ")]
    private SpriteRenderer _fadeImage;

    [SerializeField, Header("スターオブジェ")]
    private GameObject _star;

    [SerializeField, Header("発射角度")]
    private float _minAngle;

    [SerializeField]
    private float _maxAngle;

    [SerializeField, Header("星の数")]
    private int _starNum;

    // 現在の状態
    private State _currentState;

    // タイム
    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        _currentState = State.Star;

        _time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_currentState)
        {
            case State.Star:
                Star();
                break;
            case State.FadeIn:
                FadeIn();
                break;
            case State.Walk:
                Walk();
                break;
        }
    }

    private void Star()
    {
        for(int i = 0; i < _starNum; i++)
        {
            GameObject obj = Instantiate(_star, Vector3.zero, Quaternion.identity);

            float angle = Random.Range(_minAngle, _maxAngle) * Mathf.Deg2Rad;

            Vector3 v = new Vector3();

            v.x = Mathf.Cos(angle);
            v.y = Mathf.Sin(angle);

            v *= 500;

            obj.GetComponent<Rigidbody>().AddForce(v);
        }

        _currentState = State.FadeIn;
    }

    private void FadeIn()
    {
        _time += Mathf.Deg2Rad * 0.1f;

        _fadeImage.color = new Color(0, 0, 0, Mathf.Cos(_time));

        Walk();

        if(_fadeImage.color.a < 0.01f)
        {
            _currentState = State.Walk;
            _time = 0;
        }

        Debug.Log("fade");
    }

    private void Walk()
    {
        _time += Time.deltaTime;

        _camera.transform.position -= new Vector3(0, 0, _speed) * Time.deltaTime;

        if(Input.GetButtonDown("Z"))
        {
            _time += _timeLimit;
        }

        if (_time > _timeLimit)
        {
            SceneManager.LoadScene(_nextSceneName);
        }

        Debug.Log("walk");
    } 
}
