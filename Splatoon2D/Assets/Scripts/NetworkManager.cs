using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //极其重要又频频令人头疼的tilemapcontroller
    public static TilemapController tilemapcontroller;
    //cinemachine，实时跟随玩家
    public static CinemachineVirtualCamera myCinemachine;
    //是否进入房间，用于Update的判断条件
    bool IsInRoom = false;
    //玩家预制件
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
#if     UNITY_EDITOR//在编辑器模式下
        if (IsInRoom)
            print($"房间人数：{PhotonNetwork.CurrentRoom.PlayerCount}");
#endif
        if (!OnlineMode.IsReady && IsInRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2) {
            OnlineMode.IsReady = true;
        }
    }
    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
#if     UNITY_EDITOR//在编辑器模式下
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
            //实例化玩家并设置cinemachine
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
