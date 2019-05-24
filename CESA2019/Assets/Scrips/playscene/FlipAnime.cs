using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipAnime : MonoBehaviour
{
    [SerializeField, Header("スプライトレンダラー")]
    private SpriteRenderer _spriteRenderer;

    [SerializeField, Header("スプライト")]
    private Sprite[] _sprites;

    [SerializeField, Header("何秒ごとに切り替えるか")]
    private float _interval;

    private int _currentNum;

    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        _time = 0;
        _currentNum = 0;
        _spriteRenderer.sprite = _sprites[_currentNum];
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time > _interval)
        {
            if(_currentNum + 1 >= _sprites.Length)
            {
                _currentNum = 0;
            }
            else
            {
                _currentNum++;
            }

            _spriteRenderer.sprite = _sprites[_currentNum];

            _time = 0;
        }
    }
}
