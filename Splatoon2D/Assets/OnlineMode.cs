using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineMode : MonoBehaviour
{
    //�Ƿ��������ģʽ
    public static bool EndOnlineMode = false;
    //��������RPC�Ը��·���
    PhotonView photonView;
    //�����Ƿ����ߣ�����ǣ��Ϳ�ʼ��ʱ
    public static bool IsReady = false;
    //�Ƿ���ʾWaiting����
    private bool IsWaiting = true;
    //��Ϸ��������
    public GameObject End;
    //�ȴ�����
    public GameObject Waiting; 
    //��ʾ��ʤ�����
    public TMPro.TextMeshProUGUI EndText;
    //TilemapControler��������ڼ�¼����
    TilemapController tilemapcontroller;
    //��ʾ����ʱ
    public TMPro.TextMeshProUGUI TimeCount;
    //����ʱ
    public float TotalTime;
    private float curTime;
    //�����ɫ(��NetworkController������): 0:G, 1:B, 2:R
    public static int PlayerTag;
    //����
    int GScore = 0;
    int RScore = 0;
    public int TargetScore = 7500;
    // Start is called before the first frame update
    void Start()
    {
        EndOnlineMode = false;
        IsReady = false;
        curTime = TotalTime;
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
        //photonView = gameObject.GetComponent<PhotonView>();
        photonView = PhotonView.Get(this);
    }

    [PunRPC]
    void SyncScore(int Tag, int Score) {
        if (Tag == 0) {
            GScore = Score;
        } else if (Tag == 2){
            RScore = Score; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsReady) return;
        if (IsWaiting) {
            Waiting.SetActive(false);
            IsWaiting = false;
        }
        EndOnlineMode = false;
        //���·���
        if (PlayerTag == 0) {
            //GScore = tilemapcontroller.getScore(0);
            photonView.RPC("SyncScore",RpcTarget.All, 0, tilemapcontroller.getScore(0));
        } else if (PlayerTag == 2) {
            //RScore = tilemapcontroller.getScore(2);
            photonView.RPC("SyncScore",RpcTarget.All, 2, tilemapcontroller.getScore(2));
        }
        ScoreBarControllerG.ScoreBarG.setValue((float)GScore / TargetScore);
        ScoreBarControllerR.ScoreBarR.setValue((float)RScore / TargetScore);
        //���µ���ʱ
        curTime = Mathf.Clamp(curTime - Time.deltaTime, 0, TotalTime);
        TimeCount.text = $"Time: {(int)curTime}s";
        //����Ƿ�����
        if (PlayerControllerOnline.GIsDead || PlayerControllerOnline.RIsDead) {
            NoTimeOrDie(false);
        } else if (Mathf.Approximately(curTime, 0)) { //ʱ���Ƿ�ľ�
            NoTimeOrDie(true);
        } else if (GScore >= TargetScore || RScore >= TargetScore) {
            EndFunc();
        }
    }

    void EndFunc() {
        if (RScore > GScore)
            EndText.text = "The Winner is Red!";
        else if (GScore > RScore)
            EndText.text = "The Winner is Green!";
        else EndText.text = "It's a Draw!";
        GScore = 0;
        RScore = 0;
        PlayerControllerOnline.RIsDead = false;
        PlayerControllerOnline.GIsDead = false;
        PhotonNetwork.LeaveRoom();
        End.SetActive(true);
        IsWaiting = true;
        EndOnlineMode = true;
    }

    void NoTimeOrDie(bool tag) {
        if (tag) {
            EndFunc();
        } else {
            if (PlayerControllerOnline.GIsDead) {
                EndText.text = "The Winner is Red!";
            } else {
                EndText.text = "The Winner is Green!";
            }
        End.SetActive(true);
        }
        IsWaiting = true;
        PlayerControllerOnline.RIsDead = false;
        PlayerControllerOnline.GIsDead = false;
        PhotonNetwork.LeaveRoom();
        GScore = 0;
        RScore = 0;
        EndOnlineMode = true;
    }
}
