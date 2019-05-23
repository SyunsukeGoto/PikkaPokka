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
        public enum ObjectType
        {
            FloorNone = -1,     //何にもなし
            FloorWhite001, //白床001
            FloorBlue001,  //青床001
            FloorRed001,   //赤床001
            FloorGreen001, //緑床001
            FloorBlack001, //黒床001

            Num
        }

        public enum GroundType
        {
            GroundNone = -1, //何もなし
            Wall,  //壁
            Player,//プレイヤー
            Goal,   //ゴール
            Enemy, //敵
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
        private string _objFileName = "ObjectData"; //ファイルの名前(オブジェクトよう)
        [SerializeField]
        private string _stageFileName = "StageData"; //ファイルの名前(スーテジ用)
        [SerializeField]
        private int _stageNumber = 1; //ステージ番号
        private string _openFilenameExtension;
        private string _objFilePath; //オブジェクト用ファイルパス
        private string _stageFilePath; //オブジェクト用ファイルパス         
        [SerializeField]
        private GameObject[] _gameObj = new GameObject[(int)ObjectType.Num]; //オブジェクトを入れる
        [SerializeField]
        private GameObject[] _floorObj = new GameObject[(int)GroundType.Num];//地面用の生成オブジェクト
        private List<int> _objectDataList; //オブジェクトデータリスト
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

        // Start is called before the first frame update
        void Start()
        {
            startPos = this.transform.position;
            startPlayerPos = Vector3.zero;
            _searchWidth = 0;
            _openFilenameExtension = ".csv";

            _objFilePath = Application.dataPath + "/StreamingAssets" + @"\Data\StageData\" + _objFileName  + _stageNumber+ _openFilenameExtension;
            _stageFilePath = Application.dataPath+ "/StreamingAssets" + @"\Data\StageData\" + _stageFileName + _stageNumber +_openFilenameExtension;

            _objectDataList = new List<int>(); //データリスト作成  
            _stageDataList = new List<int>();  //データリスト作成
            ReadFile();//ファイルを読み込む
            SearchWidth();
            BuildFloor(); //床を作る
            BuildStage(); //オブジェクトを作る
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
                time = 0.0f; 
          
             
            }
           
            
        }
        /// <summary>
        /// 敵を作る関数
        /// </summary>
        void CreateEnemy()
        {
            for(int i = 0; i <_enemyPosList.Count; i++)
            {
                GameObject go = GameObject.Instantiate(enemy) as GameObject;
                go.GetComponent<Makoto.Enemy>()._player = player;
                go.GetComponent<Makoto.Enemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                go.transform.position = _enemyPosList[i];
            }
       
           // Debug.Log("作った！" + go.transform.position);
   
        }

        //ファイル読み込み
        public void ReadFile()
        {
            // _csvData.Clear();

            int objData;
            //　一括で取得
            string[] objTexts = File.ReadAllText(_objFilePath).Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in objTexts)
            {
                int tmp = 0;
                Int32.TryParse(text, out tmp);

                _objectDataList.Add(tmp);
            }
            objData = _objectDataList.Count;

            //デバッグ
            for (int i = 0; i < objData; i++)
            {
                Debug.Log(_objectDataList[i]);
            }

            string[] stageTexts = File.ReadAllText(_stageFilePath).Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in stageTexts)
            {
                int tmp = 0;
                Int32.TryParse(text, out tmp);

                _stageDataList.Add(tmp);
            }

        }


        //作成関数
        public void BuildStage()
        {
            for (int i = _objectDataList.Count - 1; i >= 0; i--)
            {
                if (_objectDataList[i] != -1 && _objectDataList[i] != (int)GroundType.Enemy)
                {
                    GameObject go = _gameObj[_objectDataList[i]].GetComponent<MapObjectBace>().InstanceObject(transform.position);
                    //go.transform.position = this.transform.position;
                    //プレイヤーの位置とオブジェクトを記憶
                    if(_objectDataList[i] == (int)GroundType.Player)
                    {
                        player = go;
                       // startPlayerPos = new Vector3(30.0f, 0.5f ,- 30.0f);
                        startPlayerPos = go.transform.position;
                    }
                   
                }
                else if(_objectDataList[i] == (int)GroundType.Enemy)
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
                if (_searchWidth * _searchWidth == _objectDataList.Count)
                {
                    //同じだったら抜ける
                    break;
                }

                _searchWidth++;

            }


        }
    }



}
