using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField]
    private Image _fade;

    [SerializeField, Range(1, 10)]
    private float _fadeTime;

    private float _time= 0f;

    public enum Mode
    {
        NONE,
        IN,
        OUT,
    }

    private Mode _mode;

    public Mode MODE
    {
        set
        {
            _mode = value;
            _time = 0f;
            Color color;
            switch(value)
            {
                case Mode.IN:
                    color = _fade.color;
                    color.a = 1;
                    _fade.color = color;
                    break;
                case Mode.OUT:
                    color = _fade.color;
                    color.a = 0;
                    _fade.color = color;
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _mode = Mode.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            MODE = Mode.NONE;
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            MODE = Mode.IN;
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            MODE = Mode.OUT;
        }
        Debug.Log("時間" + Time.deltaTime);
        switch(_mode)
        {
            case Mode.IN:
                FadeIn();
                break;
            case Mode.OUT:
                FadeOut();
                break;
        }
    }


    private void FadeIn()
    {
        _time += Time.deltaTime;
        float a = Mathf.Cos(_time / _fadeTime * 90 * Mathf.Deg2Rad);
        Color color = _fade.color;
        color.a = a;
        _fade.color = color;

        if(_fade.color.a <= 0.01f)
        {
            color.a = 0;
            _fade.color = color;
            _mode = Mode.NONE;
        }
    }

    private void FadeOut()
    {
        _time += Time.deltaTime;
        float a = Mathf.Sin(_time / _fadeTime * 90 * Mathf.Deg2Rad);
        Color color = _fade.color;
        color.a = a;
        _fade.color = color;

        if (_fade.color.a >= 0.99f)
        {
            color.a = 1;
            _fade.color = color;
            _mode = Mode.NONE;
        }
    }
}
