using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineMode : MonoBehaviour
{
    //是否结束在线模式
    public static bool EndOnlineMode = false;
    //用来调用RPC以更新分数
    PhotonView photonView;
    //两人是否都在线，如果是，就开始计时
    public static bool IsReady = false;
    //是否显示Waiting界面
    private bool IsWaiting = true;
    //游戏结束界面
    public GameObject End;
    //等待界面
    public GameObject Waiting; 
    //显示获胜的玩家
    public TMPro.TextMeshProUGUI EndText;
    //TilemapControler组件，用于记录分数
    TilemapController tilemapcontroller;
    //显示倒计时
    public TMPro.TextMeshProUGUI TimeCount;
    //倒计时
    public float TotalTime;
    private float curTime;
    //玩家颜色(由NetworkController来设置): 0:G, 1:B, 2:R
    public static int PlayerTag;
    //分数
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
        //更新分数
        if (PlayerTag == 0) {
            //GScore = tilemapcontroller.getScore(0);
            photonView.RPC("SyncScore",RpcTarget.All, 0, tilemapcontroller.getScore(0));
        } else if (PlayerTag == 2) {
            //RScore = tilemapcontroller.getScore(2);
            photonView.RPC("SyncScore",RpcTarget.All, 2, tilemapcontroller.getScore(2));
        }
        ScoreBarControllerG.ScoreBarG.setValue((float)GScore / TargetScore);
        ScoreBarControllerR.ScoreBarR.setValue((float)RScore / TargetScore);
        //更新倒计时
        curTime = Mathf.Clamp(curTime - Time.deltaTime, 0, TotalTime);
        TimeCount.text = $"Time: {(int)curTime}s";
        //玩家是否死亡
        if (PlayerControllerOnline.GIsDead || PlayerControllerOnline.RIsDead) {
            NoTimeOrDie(false);
        } else if (Mathf.Approximately(curTime, 0)) { //时间是否耗尽
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
