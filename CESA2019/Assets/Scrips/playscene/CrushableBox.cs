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

        // Actor: Tamamura Shuuki
        // Add: カラーコーンコンポーネントのへの参照  
        // カラーコーンが壊れる時に使います
        private ColorCone _colorCone;

        // Start is called before the first frame update
        void Start()
        {
            _createNum = 0;
            _dethflag = false;

            _colorCone = GetComponent<ColorCone>();
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

                // Actor: Tamamura Shuuki
                // Add: コーンの破壊処理を追加
                // カラーコーンを壊すエフェクトを発動し、コライダとリジッドボディを削除します。
                // オブジェクトは破棄しません
                _colorCone.Destruction();
                Destroy(GetComponent<Rigidbody>());
                Destroy(GetComponent<BoxCollider>());

                _dethflag = false;
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