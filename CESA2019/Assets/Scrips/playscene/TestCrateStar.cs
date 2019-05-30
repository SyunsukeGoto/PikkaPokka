using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCrateStar : MonoBehaviour
{
    public GameObject star;
    public bool _flag;
    [SerializeField ,Range(0.1f,1f)]
    private float _interval = 0.5f;
    int num = 5;
    float _time;
    private List<GameObject> _star;
    // Start is called before the first frame update
    void Start()
    {
        _star = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_flag)  
        {
            Debug.Log("1agrfd");
            _time += Time.deltaTime;
            if (_time >= _interval)
            {
                _time = 0f;
                for (int i = 0; i < _star.Count; i++)
                {
                    _star[i].SetActive(true);
                    _star[i].GetComponent<Light>().range = 2;
                }
            }

        }
        else
        {
            if (Time.frameCount % 60 == 0)
            {
                for (int i = 0; i < num; i++)
                {
                    GameObject go = Instantiate(star) as GameObject;
                    go.transform.position = this.transform.position + new Vector3(0, 1.0f, 0);
                    go.SetActive(false);
                    _star.Add(go);
                }
            }
        }
    }
}
