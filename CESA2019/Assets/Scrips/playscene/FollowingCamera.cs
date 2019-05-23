using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            updateAngle(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        updateDistance(Input.GetAxis("Mouse ScrollWheel"));

        var lookAtPos = target.transform.position + offset;
        updatePosition(lookAtPos);
        transform.LookAt(lookAtPos);
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
        var da = (-player.Angle + 270)* Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + distance * Mathf.Cos(dp),
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }
}
