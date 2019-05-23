using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Momoya
{

    public class CrushableBox : MonoBehaviour
    {
        private int _createNum; //生成する個数
        public GameObject _star;
        private float _createZone = 1.0f;
        private float _addPower = 200.0f;
        bool _dethflag;
        // Start is called before the first frame update
        void Start()
        {
            _createNum = 0;
            _dethflag = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (_dethflag == true)
            {
                for (int i = 0; i < _createNum; i++)
                {
                    GameObject go = Instantiate(_star) as GameObject;
                    go.transform.position = this.transform.position + new Vector3(0, 1.0f, 0);
                    go.transform.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-_createZone, _createZone), Random.Range(-_createZone, _createZone), Random.Range(-_createZone, _createZone)) * _addPower);
                }
                Destroy(this.gameObject);
            }
        }
        //死ぬための関数
        public void DethCall(int createNum)
        {
            _dethflag = true;
            _createNum = createNum;
        }
    }
}