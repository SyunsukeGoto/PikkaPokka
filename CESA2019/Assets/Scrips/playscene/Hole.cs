using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    //列挙型の定義
   public enum HoleType
    {
        Small,　//小
        Medium, //中
        Big,    //大

        Num
    }
    [SerializeField]
    HoleType _holeType = HoleType.Small;
    
    // Start is called before the first frame update
    void Start()
    {
        this.tag = "Hole";//強制的にHoleに変更

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HoleType GetType
    {
        get { return _holeType; }
    }

}
