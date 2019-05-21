using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransCameraPos : MonoBehaviour
{
    [SerializeField]
    Vector3 _offset = Vector3.zero; //微調整用のポジション
    public GameObject target;    //目標(ゲームオブジェクト)
    Vector3 targetPos;           //目標地点
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //目標地点を決める
        targetPos = new Vector3(target.transform.position.x + _offset.x, target.transform.position.y + _offset.y, target.transform.position.z + _offset.z);
        //オブジェクトのポジションを目標地点に設定
        transform.position = targetPos;
        
    }
}
