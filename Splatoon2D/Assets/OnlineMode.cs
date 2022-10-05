using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineMode : MonoBehaviour
{
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
    private int GScore = 0;
    private int RScore = 0;
    public int TargetScore = 7500;
    // Start is called before the first frame update
    void Start()
    {
        curTime = TotalTime;
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsReady) return;
        if (IsWaiting) {
            Waiting.SetActive(false);
            IsWaiting = false;
        }
        //���·���
        GScore = tilemapcontroller.getScore(0);
        RScore = tilemapcontroller.getScore(2);
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
        IsReady = false;
        End.SetActive(true);
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
        IsReady = false;
    }
}
