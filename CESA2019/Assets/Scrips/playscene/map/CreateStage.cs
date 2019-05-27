using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using UnityEngine.AI;

namespace Momoya
{
    public class CreateStage : MonoBehaviour
    {
        //列挙型の宣言
        public enum IObjectType
        {
            FloorNone = -1,//何にもなし
            FloorWhite001, //白床001
            FloorBlue001,  //青床001
            FloorRed001,   //赤床001
            FloorGreen001, //緑床001
            FloorBlack001, //黒床001

            Num
        }

        public enum MObjectType
        {
            NONE = -1,     //何にもなし
            Player, //白床001
            Enemy,  //青床001
            FloorRed001,   //赤床001
            FloorGreen001, //緑床001
            FloorBlack001, //黒床001

            Num
        }

        public enum GroundType
        {
            NONE = -1, //何もなし
            Player,  //壁
            Enemy,//プレイヤー
            Goal,   //ゴール
            HOGE, //敵
            GroundHoleSmall, //小穴
            GroundHoleMedium, //中穴
            GroundHoleBig, //大穴
            GroundHoleGoal,//ゴール床

            Num
        }

        



        //定数の定義
        [SerializeField]
        private float _width = 1;
        //変数の定義
        private Vector3 startPos;
        private int _searchWidth;

        [SerializeField]
        private string _iObjFileName = "IObjectData"; //ファイルの名前(動かないオブジェクトよう)
        [SerializeField]
        private string _mObjFileName = "MObjectData"; //ファイルの名前(動くオブジェクトよう)
        [SerializeField]
        private string _stageFileName = "StageData"; //ファイルの名前(スーテジ用)
        [SerializeField]
        private int   _stageNumber = 1; //ステージ番号
        private string _openFilenameExtension;
        private string _MobjFilePath; //オブジェクト用ファイルパス
        private string _IobjFilePath; //オブジェクト用ファイルパス
        private string _stageFilePath; //オブジェクト用ファイルパス         
        [SerializeField]
        private GameObject[] _mGameObj = new GameObject[(int)MObjectType.Num]; //オブジェクトを入れる
        [SerializeField]
        private GameObject[] _iGameObj = new GameObject[(int)IObjectType.Num];//地面用の生成オブジェクト
        [SerializeField]
        private GameObject[] _floorObj = new GameObject[(int)GroundType.Num];//地面用の生成オブジェクト
        private List<int> _mObjectDataList; //オブジェクトデータリスト
        private List<int> _iObjectDataList; //オブジェクトデータリスト
        private List<int> _stageDataList;  //ステージデータリスト
        [SerializeField]
        private GameObject enemy; //敵
        private GameObject player;//プレイヤー
        private Vector3 startPlayerPos;
        [SerializeField]
        private float time = 0.0f;
        [SerializeField]
        private float span = 5.0f;


        bool enemyFlag;
        [SerializeField]    
        NavMeshSurface navMeshSurface; //ナビメッシュ
     

        List<Vector3> _enemyPosList = new List<Vector3>(); //エネミーのポジションを把握するリスト

        GameObject test;
        GameObject _setPlayer;
        public float _top, _left, _down, _right; //移動制限
        // Start is called before the first frame update
        void Start()
        {
            startPos = this.transform.position;
            startPlayerPos = Vector3.zero;
            _searchWidth = 0;
            _openFilenameExtension = ".csv";

            _MobjFilePath = Application.dataPath + "/StreamingAssets" + @"\Data\StageData\" + _mObjFileName  + _stageNumber+ _openFilenameExtension;
            _IobjFilePath = Application.dataPath + "/StreamingAssets" + @"\Data\StageData\" + _iObjFileName  +  _stageNumber + _openFilenameExtension;
            _stageFilePath = Application.dataPath+ "/StreamingAssets" + @"\Data\StageData\" + _stageFileName + _stageNumber +_openFilenameExtension;

            _mObjectDataList = new List<int>(); //データリスト作成
            _iObjectDataList = new List<int>();  
            _stageDataList = new List<int>();  //データリスト作成
            ReadFile();//ファイルを読み込む
            SearchWidth();
            BuildFloor(); //床を作る
            BuildMObject(); //動くオブジェクトを作る
            BuildIObject();//動かないオブジェクトを作る
            time = 0.0f;

            enemyFlag = false;
            navMeshSurface.BuildNavMesh();
        }

        // Update is called once per frame
        void Update()
        {
            //特定のタイムが経過後敵を生成する
            time += Time.deltaTime;
            if(time > span && enemyFlag == false)
            {
                enemyFlag = true;
                CreateEnemy();
                _setPlayer.GetComponent<PlayerController>()._left = _left;
                _setPlayer.GetComponent<PlayerController>()._right = _right;
                _setPlayer.GetComponent<PlayerController>()._top = _top;
                _setPlayer.GetComponent<PlayerController>()._down = _down;
                time = 0.0f; 
             
            }
           
            
        }
        /// <summary>
        /// 敵を作る関数
        /// </summary>
        void CreateEnemy()
        {
            GameObject Enemys = new GameObject();
            for(int i = 0; i <_enemyPosList.Count; i++)
            {
                GameObject go = GameObject.Instantiate(enemy) as GameObject;
                go.GetComponent<Makoto.Enemy>()._player = player;
                go.GetComponent<Makoto.Enemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                go.transform.position = _enemyPosList[i];
                go.transform.parent = Enemys.transform;

                // Actor: Tamamura Shuuki
                // Add: 観測者に生成されたことを伝える
                ActorInstantiateListener.Instance.OnInstantiate(go.GetComponent<Makoto.Enemy>());
            }

            // Debug.Log("作った！" + go.transform.position);

        }

        //ファイル読み込み
        public void ReadFile()
        {
            // _csvData.Clear();
            //　一括で取得
            string[] mObjTexts = File.ReadAllText(_MobjFilePath).Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in mObjTexts)
            {
                int tmp = 0;
                Int32.TryParse(text, out tmp);

                _mObjectDataList.Add(tmp);
            }

            string[] iObjTexts = File.ReadAllText(_IobjFilePath).Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in iObjTexts)
            {
                int tmp = 0;
                Int32.TryParse(text, out tmp);

                _iObjectDataList.Add(tmp);
            }

            string[] stageTexts = File.ReadAllText(_stageFilePath).Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in stageTexts)
            {
                int tmp = 0;
                Int32.TryParse(text, out tmp);

                _stageDataList.Add(tmp);
            }

            for(int i =0;i < _iObjectDataList.Count;i++)
            {
                Debug.Log("オブジェクトデータ" + _iObjectDataList[i]);
            }
        }

        public void BuildIObject()
        {
            for (int i = _iObjectDataList.Count - 1; i >= 0; i--)
            {
                if (_iObjectDataList[i] != -1 )
                {
                    GameObject go = _iGameObj[_iObjectDataList[i]].GetComponent<MapObjectBace>().InstanceObject(transform.position);

                }

                if ((i) % _searchWidth != 0)
                {
                    transform.position = new Vector3(this.transform.position.x + _width, this.transform.position.y, this.transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(startPos.x, this.transform.position.y, this.transform.position.z - _width);
                }

            }
        }

        //作成関数
        public void BuildMObject()
        {
            for (int i = _mObjectDataList.Count - 1; i >= 0; i--)
            {
                if(i == _mObjectDataList.Count -1)
                {
                    _down = transform.position.z + 0.5f;
                    _right = transform.position.x - 0.5f;
                }

                if(i == 0)
                {
                    _top = transform.position.z - 0.5f;
                    _left = transform.position.x + 0.5f;
                  
                }


                if (_mObjectDataList[i] != -1 && _mObjectDataList[i] != (int)MObjectType.Enemy)
                {
                    GameObject go = _mGameObj[_mObjectDataList[i]].GetComponent<MapObjectBace>().InstanceObject(transform.position);
                    //go.transform.position = this.transform.position;
                    //プレイヤーの位置とオブジェクトを記憶
                    if(_mObjectDataList[i] == (int)MObjectType.Player)
                    {
                        player = go;
                        _setPlayer = go;
                        // startPlayerPos = new Vector3(30.0f, 0.5f ,- 30.0f);
                        startPlayerPos = go.transform.position;
                        go.GetComponent<PlayerController>()._createStage = this;
                        // Actor: Tamamura Shuuki
                        // Add: 観測者に生成されたことを伝える
                        ActorInstantiateListener.Instance.OnInstantiate(go.GetComponent<Player>());
                    }
                   
                }
                else if(_mObjectDataList[i] == (int)MObjectType.Enemy)
                {
                    _enemyPosList.Add(transform.position);
                }



                if ((i) % _searchWidth != 0)
                {
                    transform.position = new Vector3(this.transform.position.x + _width, this.transform.position.y, this.transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(startPos.x, this.transform.position.y, this.transform.position.z - _width);
                }

            }
            transform.position = new Vector3(startPos.x, this.transform.position.y , startPos.z);
        }

        //床を作る関数
        public void BuildFloor()
        {
            for (int i = _stageDataList.Count - 1; i >= 0; i--)
            {
                if (_stageDataList[i] != -1)
                {
                    GameObject go = Instantiate(_floorObj[_stageDataList[i]]);
                    go.transform.position = this.transform.position;
                }


                if ((i) % _searchWidth != 0)
                {
                    transform.position = new Vector3(this.transform.position.x + _width, this.transform.position.y, this.transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(startPos.x, this.transform.position.y, this.transform.position.z - _width);
                }

            }

            transform.position = new Vector3(startPos.x, this.transform.position.y + 0.5f, startPos.z);
        }

        //widthを探す関数
        public void SearchWidth()
        {
            //widthがオブジェクトデータの総数になるまで回し続ける
            while (true)
            {
                if (_searchWidth * _searchWidth == _mObjectDataList.Count)
                {
                    //同じだったら抜ける
                    break;
                }

                _searchWidth++;

            }


        }

        public int GetStageNum
        {
            get { return _stageNumber; }
        }
    }



}
