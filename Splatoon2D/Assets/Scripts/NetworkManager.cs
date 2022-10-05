using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //������Ҫ��ƵƵ����ͷ�۵�tilemapcontroller
    public static TilemapController tilemapcontroller;
    //cinemachine��ʵʱ�������
    public static CinemachineVirtualCamera myCinemachine;
    //�Ƿ���뷿�䣬����Update���ж�����
    bool IsInRoom = false;
    //���Ԥ�Ƽ�
    public GameObject GreenPlayer;
    public GameObject RedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
        myCinemachine = GameObject.FindGameObjectWithTag("VCM").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update() {
#if     UNITY_EDITOR//�ڱ༭��ģʽ��
        if (IsInRoom)
            print($"����������{PhotonNetwork.CurrentRoom.PlayerCount}");
#endif
        if (!OnlineMode.IsReady && IsInRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            OnlineMode.IsReady = true;
        }
    }
    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
#if     UNITY_EDITOR//�ڱ༭��ģʽ��
        print("Conneted!");
#endif

        RoomOptions roomoptions = new RoomOptions();
        roomoptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomoptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        IsInRoom = true;
        if (GreenPlayer == null || RedPlayer == null) return;
        if (PhotonNetwork.IsMasterClient) {
            //ʵ������Ҳ�����cinemachine
            GameObject insted = PhotonNetwork.Instantiate(GreenPlayer.name, GreenPlayer.transform.position, GreenPlayer.transform.rotation);
            myCinemachine.m_Follow = insted.transform;
            OnlineMode.PlayerTag = 0;
        } else {
            GameObject insted = PhotonNetwork.Instantiate(RedPlayer.name, RedPlayer.transform.position, RedPlayer.transform.rotation);
            myCinemachine.m_Follow = insted.transform;
            OnlineMode.PlayerTag = 2;
        }
    }
}
