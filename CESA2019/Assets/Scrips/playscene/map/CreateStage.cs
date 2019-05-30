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
        //エネミーの場所
        List<Vector3> _enemy1List;
        List<Vector3> _enemy2List;
        List<Vector3> _enemy3List;
        List<Vector3> _enemy4List;
        List<Vector3> _enemy5List;
        List<Vector3> _enemy6List;
        List<Vector3> _enemy7List;
        List<Vector3> _enemy8List;
        List<Vector3> _enemy9List;
        //チェックポイントの場所
        List<GameObject> _check1List;
        List<GameObject> _check2List;
        List<GameObject> _check3List;
        List<GameObject> _check4List;
        List<GameObject> _check5List;
        List<GameObject> _check6List;
        List<GameObject> _check7List;
        List<GameObject> _check8List;
        List<GameObject> _check9List;

        Vector3 _checkStartPos;
        Vector3 _checkFnishPos;//終始ポイント

        private Vector3 _middlePoint;//中間ポイント
        // Start is called before the first frame update
        void Start()
        {
            startPos = this.transform.position;
            startPlayerPos = Vector3.zero;
            _searchWidth = 0;
            _openFilenameExtension = ".csv";

            _MobjFilePath = Application.dataPath + "/StreamingAssets" + @"\Data\StageData\" + _mObjFileName  + SharedData.GetStageNum() + _openFilenameExtension;
            _IobjFilePath = Application.dataPath + "/StreamingAssets" + @"\Data\StageData\" + _iObjFileName +  SharedData.GetStageNum() + _openFilenameExtension;
            _stageFilePath = Application.dataPath+ "/StreamingAssets" + @"\Data\StageData\" + _stageFileName + SharedData.GetStageNum() + _openFilenameExtension;
            //エネミーの場所を記憶するオブジェクト
            _enemy1List = new List<Vector3>();
            _enemy2List = new List<Vector3>();
            _enemy3List = new List<Vector3>();
            _enemy4List = new List<Vector3>();
            _enemy5List = new List<Vector3>();
            _enemy6List = new List<Vector3>();
            _enemy7List = new List<Vector3>();
            _enemy8List = new List<Vector3>();
            _enemy9List = new List<Vector3>();
            //チェックポイントの場所を記憶するオブジェクト
            _check1List = new List<GameObject>();
            _check2List = new List<GameObject>();
            _check3List = new List<GameObject>();
            _check4List = new List<GameObject>();
            _check5List = new List<GameObject>();
            _check6List = new List<GameObject>();
            _check7List = new List<GameObject>();
            _check8List = new List<GameObject>();
            _check9List = new List<GameObject>();
            
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
                for (int i = 0; i < _check1List.Count; i++)
                {
                    Debug.Log("全部" + _check1List[i].transform.position);
                }


                enemyFlag = true;
                CreateEnemy();
                _setPlayer.GetComponent<PlayerController>()._left = _left;
                _setPlayer.GetComponent<PlayerController>()._right = _right;
                _setPlayer.GetComponent<PlayerController>()._top = _top;
                _setPlayer.GetComponent<PlayerController>()._down = _down;
              
            }

            if(time > span + 0.5f && enemyFlag == false)
            {

                time = 0.0f;
            }

           
            
        }
        /// <summary>
        /// 敵を作る関数
        /// </summary>
        void CreateEnemy()
        {
            GameObject Enemys = new GameObject();

            //エネミー1にチェックポイントを入れる
            for(int i = 0; i < _enemy1List.Count;i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[1]) as GameObject;
                go.transform.position = _enemy1List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check1List.Count];
                for(int j = 0; j <_check1List.Count;j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check1List[j];
                 
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                   


                }
             
            }

            //エネミー2にチェックポイントを入れる
            for (int i = 0; i <_enemy2List.Count; i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[2]) as GameObject;
                go.transform.position = _enemy2List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check2List.Count];
                for (int j = 0; j < _check2List.Count; j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check2List[j];
                   
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                  
                }
       
            }
            //エネミー3にチェックポイントを入れる
            for (int i = 0; i < _enemy3List.Count; i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[3]) as GameObject;
                go.transform.position = _enemy3List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check3List.Count];
                for (int j = 0; j < _check3List.Count; j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check3List[j];

                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                  

                }
               
            }
            //エネミー4にチェックポイントを入れる

            for (int i = 0; i < _enemy4List.Count; i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[4]) as GameObject;
                go.transform.position = _enemy4List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check4List.Count];
                for (int j = 0; j < _check4List.Count; j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check4List[j];
              
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                  
                }

              
            }

            //エネミー5にチェックポイントを入れる
            for (int i = 0; i < _enemy5List.Count; i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[5]) as GameObject;
                go.transform.position = _enemy5List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check5List.Count];
                for (int j = 0; j < _check5List.Count; j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check5List[j];
             
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                    
                }
               
            }
            //エネミー6にチェックポイントを入れる
            for (int i = 0; i < _enemy6List.Count; i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[6]) as GameObject;
                go.transform.position = _enemy6List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check6List.Count];
                for (int j = 0; j < _check6List.Count; j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check6List[j];
        
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                   
                }

   
            }

            //エネミー7にチェックポイントを入れる
            for (int i = 0; i < _enemy7List.Count; i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[7]) as GameObject;
                go.transform.position = _enemy7List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check7List.Count];
                for (int j = 0; j < _check7List.Count; j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check7List[j];
                
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                   
                }
          
            }

            //エネミー8にチェックポイントを入れる
            for (int i = 0; i < _enemy8List.Count; i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[8]) as GameObject;
                go.transform.position = _enemy8List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check8List.Count];
                for (int j = 0; j < _check8List.Count; j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check8List[j];
                
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                   
                }
               
            }
            //エネミー9にチェックポイントを入れる
            for (int i = 0; i < _enemy9List.Count; i++)
            {
                GameObject go = GameObject.Instantiate(_mGameObj[9]) as GameObject;
                go.transform.position = _enemy9List[i];
                go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA = new GameObject[_check9List.Count];
                for (int j = 0; j < _check9List.Count; j++)
                {
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._patrolPointA[j] = _check9List[j];
            
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._player = player;
                    go.transform.GetComponent<Makoto.PatrolEnemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
                   
                }
             
            }

            //for (int i = 0; i <_enemyPosList.Count; i++)
            //{
            //    GameObject go = GameObject.Instantiate(enemy) as GameObject;
            //    go.GetComponent<Makoto.Enemy>()._player = player;
            //    go.GetComponent<Makoto.Enemy>()._starMove = player.GetComponent<PlayerController>()._starMove;
            //    go.transform.position = _enemyPosList[i];
            //    go.transform.parent = Enemys.transform;

            //    // Actor: Tamamura Shuuki
            //    // Add: 観測者に生成されたことを伝える
            //    ActorInstantiateListener.Instance.OnInstantiate(go.GetComponent<Makoto.Enemy>());
            //}

            // Debug.Log("作った！" + go.transform.position);
            _checkStartPos.y = 0.0f;
            _checkFnishPos.y = 0.0f;
            player.transform.GetComponent<PlayerController>()._distance.StartPos = _checkStartPos;
            player.transform.GetComponent<PlayerController>()._distance.GoalPos = _checkFnishPos;

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
                    if (_iObjectDataList[i] == 1|| _iObjectDataList[i] == 2)
                    {
                        _checkStartPos = go.transform.position;
                    }

                    if (_iObjectDataList[i] == 7 || _iObjectDataList[i] == 8)
                    {
                        _checkFnishPos = go.transform.position;
                    }
                    //go.transform.parent = transform;
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


                if (_mObjectDataList[i] != -1 && (_mObjectDataList[i] > 9 || _mObjectDataList[i] == 0))
                {
                    GameObject go = _mGameObj[_mObjectDataList[i]].GetComponent<MapObjectBace>().InstanceObject(transform.position);
                    //go.transform.position = this.transform.position;
                    //プレイヤーの位置とオブジェクトを記憶
                    if(_mObjectDataList[i] == 0 || _mObjectDataList[i] == 19 || _mObjectDataList[i] == 20)
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

                    if(_mObjectDataList[i] > 9 && _mObjectDataList[i] <=18)//チェックポイントの場合入れる
                    {
                        Debug.Log("obj" + _mObjectDataList[i]);
                        switch (_mObjectDataList[i])
                        {
                            case 10: _check1List.Add(go); break;
                            case 11: _check2List.Add(go); break;
                            case 12: _check3List.Add(go); break;
                            case 13: _check4List.Add(go); break;
                            case 14: _check5List.Add(go); break;
                            case 15: _check6List.Add(go); break;
                            case 16: _check7List.Add(go); break;
                            case 17: _check8List.Add(go); break;
                            case 18: _check9List.Add(go); break;

                        }
                    }
                   
                }
                else if(_mObjectDataList[i] >= 1) //エネミーの場合ポジションの入れる
                {
                    switch(_mObjectDataList[i])
                    {
                        case 1: _enemy1List.Add(transform.position); break;
                        case 2: _enemy2List.Add(transform.position); break;
                        case 3: _enemy3List.Add(transform.position); break;
                        case 4: _enemy4List.Add(transform.position); break;
                        case 5: _enemy5List.Add(transform.position); break;
                        case 6: _enemy6List.Add(transform.position); break;
                        case 7: _enemy7List.Add(transform.position); break;
                        case 8: _enemy8List.Add(transform.position); break;
                        case 9: _enemy9List.Add(transform.position); break;
                       
                    }
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
            int middleCount = _stageDataList.Count / 2;

            for (int i = _stageDataList.Count - 1; i >= 0; i--)
            {
                //真ん中をゲット
                if(middleCount == i)
                {
                    _middlePoint = transform.position;
                }

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

        public Vector3 GetMiddle
        {
            get { return _middlePoint; }
        }


    }



}
