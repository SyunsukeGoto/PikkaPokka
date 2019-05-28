using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Momoya
{

public class CeackBreakObject : MonoBehaviour
{
    [SerializeField]
     GameObject _player;    //プレイヤー

     public GameObject crushableBox;//壊せる箱
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "CrushableBox")
            {
                crushableBox = other.gameObject;
                _player.GetComponent<Momoya.PlayerController>().StrikeMode = true;
                _player.GetComponent<Momoya.PlayerController>().CrushableBox = crushableBox;

            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "CrushableBox")
            {

                _player.GetComponent<Momoya.PlayerController>().StrikeMode = false;
                _player.GetComponent<Momoya.PlayerController>().CrushableBox = null;
                crushableBox = null;

            }
        }

    }

}
