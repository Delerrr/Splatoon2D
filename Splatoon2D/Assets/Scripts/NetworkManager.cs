using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
/*    //������ҵ�tilemapController������ͬ����ɫ
    public TilemapController tilemapcontrollerR;
    public TilemapController tilemapcontrollerG;
*/

    //������
    public static TilemapController tilemapcontroller;
    //����ʵ����Tilemap
    public GameObject Grid;

    //cinemachine��ʵʱ�������
    public static CinemachineVirtualCamera myCinemachine;
    //�Ƿ���뷿�䣬����Update���ж�����
    bool IsInRoom = false;
    //������Ҹ��Եķ���
    private int GScore = 0;
    private int RScore = 0;
    //���Ԥ�Ƽ�
    public GameObject GreenPlayer;
    public GameObject RedPlayer;

/*    //ͬ����ͼ��ɫ
    public static void SyncColor(Vector3Int Pos, Color newColor) {
        tilemapcontroller.UpdateColorSync(Pos, newColor);
    }
*/    //ͬ������
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
#if     UNITY_EDITOR//�ڱ༭��ģʽ��
/*        if (IsInRoom)
            print($"����������{PhotonNetwork.CurrentRoom.PlayerCount}");
*/
#endif
        if (!OnlineMode.IsReady && IsInRoom && PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            OnlineMode.IsReady = true;
        } 
    }
    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
#if     UNITY_EDITOR//�ڱ༭��ģʽ��
        print("Connected!");
#endif

        RoomOptions roomoptions = new RoomOptions();
        roomoptions.MaxPlayers = 2;
#if UNITY_EDITOR
        bool CreateRoom = PhotonNetwork.JoinOrCreateRoom("TestRoom", roomoptions, TypedLobby.Default);
        print($"Create Room State:{CreateRoom}");
#else
        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomoptions, TypedLobby.Default);
#endif
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
#if UNITY_EDITOR 
        print("Joined Room!");
#endif
        IsInRoom = true;
        if (GreenPlayer == null || RedPlayer == null) return;
/*        //ʵ����Tilemap
        PhotonNetwork.Instantiate(Grid.name, Grid.transform.position, Grid.transform.rotation);
        tilemapcontroller = GetComponent<TilemapController>();
*/
        if (PhotonNetwork.IsMasterClient) {
            //ʵ������Ҳ�����cinemachine
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

    //colorname���Լ�����ɫ��Ҫ�ö��ֵ�tilemapcontroller��д����ֵ�������
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