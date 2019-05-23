using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChange : MonoBehaviour
{
    [SerializeField, Header("サイズ範囲")]
    private float _minSize;

    [SerializeField]
    private float _maxSize;

    private float _value;

    // Start is called before the first frame update
    void Start()
    {
        _value = 0.001f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(_value, _value);

        if(transform.localScale.x < _minSize || transform.localScale.x > _maxSize)
        {
            _value *= -1;
        }
    }
}
