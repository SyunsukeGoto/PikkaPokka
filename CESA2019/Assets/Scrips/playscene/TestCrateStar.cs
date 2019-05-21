using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCrateStar : MonoBehaviour
{
    public GameObject star;
    int num = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < num; i++)
            {
                GameObject go = Instantiate(star) as GameObject;
                go.transform.position = this.transform.position + new Vector3(0, 1.0f, 0);
                go.transform.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f)) * 200);
            }

        }
    }
}
