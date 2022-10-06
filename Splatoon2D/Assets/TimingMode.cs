using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingMode : MonoBehaviour
{
    //��Ϸʧ��ʱ��ʾGameOver����
    public GameObject GameOver;
    //��Ϸ��ʤʱ��ʾWin����
    public GameObject Win;
    //��ʾ����������ʱ�䵽����
    public TMPro.TextMeshProUGUI GameOverText;
    //�����ɫ����int��ʾ�����ڵ���tilemapcontroller.GetScore��
    //0��G��1��B��2��R
    public int PlayerColorTag;
    //TilemapControler��������ڼ�¼����
    TilemapControllerLocal tilemapcontroller;
    //��ʾ����ʱ
    public TMPro.TextMeshProUGUI TimeCount;
    //����ʱ
    public float TotalTime;
    private float curTime;
    //����
    private int Score = 0;
    public int TargetScore = 7500;
    // Start is called before the first frame update
    void Start()
    {
        curTime = TotalTime;
        tilemapcontroller = gameObject.GetComponent<TilemapControllerLocal>();
    }


    // Update is called once per frame
    void Update()
    {
        //���·���
        Score = tilemapcontroller.getScore(PlayerColorTag);
        ScoreBarController.ScoreBar.setValue((float)Score / TargetScore);
        //���µ���ʱ
        curTime = Mathf.Clamp(curTime - Time.deltaTime, 0, TotalTime);
        TimeCount.text = $"Time: {(int)curTime}s";
        //����Ƿ�����
        if (PlayerController.IsDead) {
            NoTimeOrDie(false);
        } else if (Mathf.Approximately(curTime, 0)) { //ʱ���Ƿ�ľ�
            NoTimeOrDie(true);
        } else if (Score >= TargetScore) {
            WinFunc();
        }
    }

    void WinFunc() {
        Win.SetActive(true);
    }

    void NoTimeOrDie(bool tag) {
        if (tag) {
            GameOverText.text = "Time IS Out!";
            GameOver.SetActive(true);
        } else {
            GameOverText.text = "You Have Died!";
            GameOver.SetActive(true);
            PlayerController.IsDead = false;
        }
    }
}
