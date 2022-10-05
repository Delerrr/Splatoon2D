using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //极其重要又频频令人头疼的tilemapcontroller，不解释
    public static TilemapController tilemapcontroller;
    //cinemachine，实时跟随玩家
    public static CinemachineVirtualCamera myCinemachine;
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

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        print("Conneted!");

        RoomOptions roomoptions = new RoomOptions();
        roomoptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomoptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        print("Joined!");
        if (GreenPlayer == null || RedPlayer == null) return;
        if (!PhotonNetwork.IsMasterClient) {
            //实例化玩家并设置cinemachine
            GameObject insted = PhotonNetwork.Instantiate(GreenPlayer.name, GreenPlayer.transform.position, GreenPlayer.transform.rotation);
            myCinemachine.m_Follow = insted.transform;
        } else {
            GameObject insted = PhotonNetwork.Instantiate(RedPlayer.name, RedPlayer.transform.position, RedPlayer.transform.rotation);
            myCinemachine.m_Follow = insted.transform;
        }
    }
}
