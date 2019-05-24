using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDirector : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _gameOver = new GameObject[8];

    [SerializeField]
    private float _minX, _maxX, _minY, _maxY;

    [SerializeField]
    private string _nextSceneName;

    [SerializeField]
    private float _nextSceneTime;

    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 8; i++)
        {
            //_gameOver[i].transform.position += _startPos[i];
            _gameOver[i].transform.position += new Vector3(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if(_time > _nextSceneTime)
        {
            //SceneManager.LoadScene(_nextSceneName);
        }
    }
}
