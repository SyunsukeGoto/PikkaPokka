using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private Vector3[] _exitPos = new Vector3[2];

    [SerializeField]
    private Vector3[] _selectPos = new Vector3[3];

    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite[] _sprites = new Sprite[2];

    [SerializeField]
    private Fade _fade;

    [SerializeField]
    private Image _hammer;

    private float _lastTrigger;

    public enum Mode
    {
        Exit,
        Select,
    }

    [SerializeField]
    private Mode _mode;

    private enum Choice
    {
        Decision,
        ReStart,
        Return,
    }

    private int _choice;

    public enum State
    {
        NONE,
        DISPLAY,
    }

    [SerializeField]
    private State _state;

    public State STATE
    {
        set
        {
            _state = value;
            if(value == State.NONE)
            {
                Time.timeScale = 1;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _image.sprite = _sprites[(int)_mode];
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ちんちん" + _state);
        if (_state == State.DISPLAY)
        {
            Color color = _image.color;
            color.a = 1;
            _image.color = color;
            _hammer.color = color;

            Time.timeScale = 0;

            switch (_mode)
            {
                case Mode.Exit:
                    Exit();
                    break;
                case Mode.Select:
                    Select();
                    break;
            }
        }
        else
        {
            Color color = _image.color;
            color.a = 0;
            _image.color = color;
            _hammer.color = color;
    
            if(Input.GetButtonDown("Start"))
            {
                _state = State.DISPLAY;
            }
        }
    }

    private void Exit()
    {
        if (Input.GetAxis("Vertical") <= -1 || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_choice != (int)Choice.Decision)
            {
                _choice -= 2;
            }
        }
        else if (Input.GetAxis("Vertical") >= 1 || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_choice != (int)Choice.Return)
            {
                _choice += 2;
            }
        }
        else if (Input.GetButtonDown("Z"))
        {
            switch ((Choice)_choice)
            {
                case Choice.Decision:
                    Application.Quit();
                    break;
                case Choice.Return:
                    STATE = State.NONE;
                    break;
            }
        }

        switch ((Choice)_choice)
        {
            case Choice.Decision:
                _hammer.GetComponent<RectTransform>().localPosition = _exitPos[0];
                break;
            case Choice.Return:
                _hammer.GetComponent<RectTransform>().localPosition = _exitPos[1];
                break;
        }
    }

    private void Select()
    {
        if (Input.GetAxis("Vertical") <= -1 || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_choice != (int)Choice.Decision)
            {
                if (_lastTrigger > -1)
                {
                    _choice--;
                }
            }
        }
        else if (Input.GetAxis("Vertical") >= 1 || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_choice != (int)Choice.Return)
            {
                if (_lastTrigger < 1)
                {
                    _choice++;
                }
            }
        }
        else if (Input.GetButtonDown("Z"))
        {
            switch ((Choice)_choice)
            {
                case Choice.Decision:
                    SceneManager.LoadScene("Select");
                    break;
                case Choice.ReStart:
                    string name = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(name);
                    break;
                case Choice.Return:
                    STATE = State.NONE;
                    break;
            }
        }

        switch((Choice)_choice)
        {
            case Choice.Decision:
                _hammer.GetComponent<RectTransform>().localPosition = _selectPos[0];
                break;
            case Choice.ReStart:
                _hammer.GetComponent<RectTransform>().localPosition = _selectPos[1];
                break;
            case Choice.Return:
                _hammer.GetComponent<RectTransform>().localPosition = _selectPos[2];
                break;
        }

        _lastTrigger = Input.GetAxis("Vertical");
    }


}
