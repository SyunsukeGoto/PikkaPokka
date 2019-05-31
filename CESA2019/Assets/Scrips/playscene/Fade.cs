using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField]
    private Image _fade;

    [SerializeField, Range(1, 10)]
    private float _fadeTime;

    private string _nextSceneName;

    private float _time= 0f;

    public enum Mode
    {
        NONE,
        IN,
        OUT,
    }

    private Mode _mode;

    private Mode MODE
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

    public void SetFadeIn()
    {
        MODE = Mode.IN;
    }

    public void SetFadeOut(string sceneName)
    {
        MODE = Mode.OUT;
        _nextSceneName = sceneName;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("時間" + Time.timeScale);
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
        Debug.Log("フェードイン");
        Debug.Log("aaaaa" + Time.timeScale);
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
        Debug.Log("フェードアウト");
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
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}
