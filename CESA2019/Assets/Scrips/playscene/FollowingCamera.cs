using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode, DisallowMultipleComponent]
public class FollowingCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject target; // an object to follow

    public GameObject Target
    {
        set { target = value; }
    }

    [SerializeField]
    private Vector3 offset; // offset form the target object

    [SerializeField]
    private float distance = 10.0f; // distance from following object
    [SerializeField]
    private float polarAngle = 65.0f; // angle with y-axis
    [SerializeField]
    private float azimuthalAngle = 270.0f; // angle with x-axis
    [SerializeField]
    private Momoya.PlayerController player;

    public Fade _fade;

    public enum Mode
    {
        Default,    // 通常
        Clear,      // クリア
    }

    private Mode _mode = Mode.Default;

    public Mode MODE
    {
        set
        {
            _mode = value;

            _star._flag = true;
            _middle = new GameObject();
            _middle.transform.position = target.GetComponent<Momoya.PlayerController>()._createStage.GetMiddle;
        }
    }

    public Quaternion Angle
    {
        get { return Quaternion.Euler(0, -azimuthalAngle, 0); }
    }

    [Space()]

    [SerializeField]
    private float minDistance = 2.0f;
    [SerializeField]
    private float maxDistance = 40.0f;
    [SerializeField]
    private float minPolarAngle = 5.0f;
    [SerializeField]
    private float maxPolarAngle = 80.0f;
    [SerializeField]
    private float mouseXSensitivity = 5.0f;
    [SerializeField]
    private float mouseYSensitivity = 5.0f;
    [SerializeField]
    private float scrollSensitivity = 5.0f;
    [SerializeField]
    private float _rotationSpped = 1.0f;

    [SerializeField]
    private TestCrateStar _star;

    private float _time = 0.0f;

    private bool _flag = false;


    private void Start()
    {

            //azimuthalAngle = target.GetComponent<Momoya.PlayerController>()._playerAngle;

    }

    private GameObject _middle;

    void LateUpdate()
    {
        if (Time.timeScale == 1)
        {
            if (_mode == Mode.Default)
            {
                azimuthalAngle += -Input.GetAxis("Turn") * _rotationSpped;

                if (Input.GetButton("LB"))
                {
                    azimuthalAngle += 3;
                }
                else if (Input.GetButton("RB"))
                {
                    azimuthalAngle -= 3;
                }

                Debug.Log("確認しまーす" + Input.GetAxis("Turn"));
                //polarAngle = Mathf.Clamp(0, minPolarAngle, maxPolarAngle);
                //updateDistance(Input.GetAxis("Mouse ScrollWheel"));

                var lookAtPos = target.transform.position + offset;
                updatePosition(lookAtPos);
                transform.LookAt(lookAtPos);

                // テスト用
                //if (Input.GetKeyDown(KeyCode.Q))
                //{
                //    _mode = Mode.Clear;
                //    _star._flag = true;
                //    _middle = new GameObject();
                //    _middle.transform.position = target.GetComponent<Momoya.PlayerController>()._createStage.GetMiddle;
                //}
            }
            else
            {
                _time += Time.deltaTime;

                distance = Mathf.Lerp(distance, 30, _time / 10);
                polarAngle = Mathf.Lerp(polarAngle, 25, _time / 10);
                azimuthalAngle += 0.5f;
                var lookAtPos = _middle.transform.position;
                updatePosition(lookAtPos);
                transform.LookAt(lookAtPos);

                if (_time >= 5 && !_flag)
                {
                    _fade.SetFadeOut("ClearScene");
                    _flag = true;
                }
            }
        }
    }

    void updateAngle(float x, float y)
    {
        x = azimuthalAngle - x * mouseXSensitivity;
        azimuthalAngle = Mathf.Repeat(x, 360);

        y = polarAngle + y * mouseYSensitivity;
        polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
    }

    void updateDistance(float scroll)
    {
        scroll = distance - scroll * scrollSensitivity;
        distance = Mathf.Clamp(scroll, minDistance, maxDistance);
    }

    void updatePosition(Vector3 lookAtPos)
    {
        var da = (azimuthalAngle)* Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + distance * Mathf.Cos(dp),
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }
}
