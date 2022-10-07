using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
/*    //两个玩家的tilemapController，用于同步颜色
    public TilemapController tilemapcontrollerR;
    public TilemapController tilemapcontrollerG;
*/


    //等待时显示的文字
    public TMPro.TextMeshProUGUI WaitingText;
    //不解释
    public static TilemapController tilemapcontroller;
    //用于实例化Tilemap
    public GameObject Grid;

    //cinemachine，实时跟随玩家
    public static CinemachineVirtualCamera myCinemachine;
    //是否进入房间，用于Update的判断条件
    bool IsInRoom = false;
    //是否已经设置过IsReady，防止重复设置
    bool HaveSetIsReady = false;
/*    //两个玩家各自的分数
    private int GScore = 0;
    private int RScore = 0;
*/    //玩家预制件
    public GameObject GreenPlayer;
    public GameObject RedPlayer;

/*    //同步地图颜色
    public static void SyncColor(Vector3Int Pos, Color newColor) {
        tilemapcontroller.UpdateColorSync(Pos, newColor);
    }
*/    //同步分数
/*    public static void SyncScore(int Tag, int Score) {
        if (Tag == 0) {
            OnlineMode.GScore = Score;
        } else if (Tag == 2){
            OnlineMode.RScore = Score; 
        }
    }
*/    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        myCinemachine = GameObject.FindGameObjectWithTag("VCM").GetComponent<CinemachineVirtualCamera>();
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
    }

    private void Update() {
        if (!OnlineMode.IsReady && IsInRoom && PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2 && !HaveSetIsReady && OnlineMode.EndOnlineMode == false) {
            OnlineMode.IsReady = true;
            HaveSetIsReady = true;
        }
        if (OnlineMode.EndOnlineMode == true)
            OnlineMode.IsReady = false;
    }
    public override void OnConnectedToMaster() {
        WaitingText.text = "Connected to Master";
        base.OnConnectedToMaster();
#if     UNITY_EDITOR//在编辑器模式下
        print("Connected!");
#endif

        RoomOptions roomoptions = new RoomOptions();
        roomoptions.MaxPlayers = 2;
#if UNITY_EDITOR
        bool Created = PhotonNetwork.JoinOrCreateRoom("TestRoom", roomoptions, TypedLobby.Default);
        print($"Create Room State:{Created}");
#else
        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomoptions, TypedLobby.Default);
#endif
        WaitingText.text = "Joining Room ...";
    }

    public override void OnJoinedRoom() {
        WaitingText.text = "Joined Room!\n Waiting For Your Opponent.";
        base.OnJoinedRoom();
#if UNITY_EDITOR 
        print("Joined Room!");
#endif
#if     UNITY_EDITOR//在编辑器模式下
        if (PhotonNetwork.CurrentRoom != null)
            print($"房间人数：{PhotonNetwork.CurrentRoom.PlayerCount}");
#endif
        IsInRoom = true;
        if (GreenPlayer == null || RedPlayer == null) return;
/*        //实例化Tilemap
        PhotonNetwork.Instantiate(Grid.name, Grid.transform.position, Grid.transform.rotation);
        tilemapcontroller = GetComponent<TilemapController>();
*/
        if (PhotonNetwork.IsMasterClient) {
            //实例化玩家并设置cinemachine
            GameObject ins = PhotonNetwork.Instantiate(GreenPlayer.name, GreenPlayer.transform.position, GreenPlayer.transform.rotation);
            myCinemachine.m_Follow = ins.transform;
            //tilemapcontrollerG = ins.GetComponent<TilemapController>();
            OnlineMode.PlayerTag = 0;
        } else {
            GameObject ins = PhotonNetwork.Instantiate(RedPlayer.name, RedPlayer.transform.position, RedPlayer.transform.rotation);
            myCinemachine.m_Follow = ins.transform;
            //tilemapcontrollerR = ins.GetComponent<TilemapController>();
            OnlineMode.PlayerTag = 2;
        }
    }

    //colorname是自己的颜色，要用对手的tilemapcontroller来写入对手的数据中
/*    public static void UpdateOpponentScore(string colorname, int newscore) {
        if (colorname == "Green") {
            tilemapcontrollerR.SetOpponetScore(colorname, newscore);
        } else {
            instantiatedR.SetOpponetScore(colorname, newscore);
        }
    }
*/
    public static GameObject InstanstiateAndLaunch(string name, Vector3 pos, Quaternion rot) {
        return PhotonNetwork.Instantiate(name, pos, rot);
    }

    public static void DestroyGameObject(GameObject obj) {
        PhotonNetwork.Destroy(obj);
    }

    //public void GetTileMapController(TilemapController other, )
}
