using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Makoto
{
    public class FlipAnime : MonoBehaviour
    {
        [SerializeField, Header("スプライトレンダラー")]
        private SpriteRenderer _spriteRenderer;

        [SerializeField, Header("イメージ")]
        private Image _image;

        [SerializeField, Header("スプライト")]
        private Sprite[] _sprites;

        [SerializeField, Header("何秒ごとに切り替えるか")]
        private float _interval;

        enum Mode
        {
            SpriteRenderer,
            Image,
        }

        [SerializeField, Header("モード")]
        private Mode _mode;
    
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
                if (_currentNum + 1 >= _sprites.Length)
                {
                    _currentNum = 0;
                }
                else
                {
                    _currentNum++;
                }

                switch (_mode)
                {
                    case Mode.SpriteRenderer:
                        _spriteRenderer.sprite = _sprites[_currentNum];
                        break;
                    case Mode.Image:
                        _image.sprite = _sprites[_currentNum];
                        break;
                }

                _time = 0;
            }
        }
    }
}
