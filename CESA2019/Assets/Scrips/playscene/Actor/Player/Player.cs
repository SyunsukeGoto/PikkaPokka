//
// Player.cs
// Actor: Tamamura Shuuki
//


public class Player : Actor
{

    // プレイヤーオブジェクトにアタッチされているコンポーネントの参照
    private Momoya.PlayerController _playerController;
    private Makoto.PlayerAnime _playerAnim;


    private void Start()
    {
        _playerController = GetComponent<Momoya.PlayerController>();
        _playerAnim = GetComponent<Makoto.PlayerAnime>();

        _type = ActorType.PLAYER;   // アクタータイプはプレイヤーに
    }

    private void Update()
    {
        
    }
}
