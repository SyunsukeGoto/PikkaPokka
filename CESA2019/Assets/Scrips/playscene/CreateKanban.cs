using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateKanban : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _gameObject;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 tmp = new Vector3(this.transform.position.x, 1.5f, transform.position.z);
        GameObject go = Instantiate(_gameObject[SharedData.GetStageNum()]) as GameObject;
        go.transform.position = tmp;
        go.transform.localScale = new Vector3(0.3f, 0.5f, 0.3f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
