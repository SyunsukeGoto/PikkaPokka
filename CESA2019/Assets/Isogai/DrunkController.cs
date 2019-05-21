using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isogai
{
    public class DrunkController : MonoBehaviour
    {
        [SerializeField]
        public Transform playerTarget;   // 追いかける対象
        private float speed = 0.01f;      // 移動スピード
        
        [SerializeField]
        private GameObject _moveTarget;    // 移動する場所
        //private Transform moveTarget;

        private GameObject _starMoveObject;

        // Start is called before the first frame update
        void Start()
        {
            _starMoveObject = GameObject.Find("Star");
            _moveTarget.transform.position = new Vector3(Random.Range(transform.position.x - 5.0f, transform.position.x + 5.0f), transform.position.y, Random.Range(transform.position.z - 5.0f, transform.position.z + 5.0f));
        }

        // Update is called once per frame
        void Update()
        {
            // 星が出ていたら
            if (_starMoveObject.GetComponent<Goto.StarMove>().GetStarFlag().IsFlag((uint)Goto.StarMove.StarFlag.GENERATE_STATE))
            {
                // targetの方に少しずつ向きが変わる
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTarget.position - transform.position), 0.3f);

                // targetに向かって進む
                transform.position += transform.forward * speed;
            }

            // 星が出ていなかったら
            if (!_starMoveObject.GetComponent<Goto.StarMove>().GetStarFlag().IsFlag((uint)Goto.StarMove.StarFlag.GENERATE_STATE))
            {
                // targetの方に少しずつ向きが変わる
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_moveTarget.transform.position - transform.position), 0.3f);

                // targetに向かって進む
                transform.position += transform.forward * speed;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.transform.name == "moveTarget")
            {
                _moveTarget.transform.position = new Vector3(Random.Range(transform.position.x - 5.0f, transform.position.x + 5.0f), transform.position.y, Random.Range(transform.position.z - 5.0f, transform.position.z + 5.0f));
            }
        }
    }
}
