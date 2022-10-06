using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingMode : MonoBehaviour
{
    //游戏失败时显示GameOver界面
    public GameObject GameOver;
    //游戏获胜时显示Win界面
    public GameObject Win;
    //显示“死亡”或“时间到！”
    public TMPro.TextMeshProUGUI GameOverText;
    //玩家颜色（用int表示，用于调用tilemapcontroller.GetScore）
    //0：G，1：B，2：R
    public int PlayerColorTag;
    //TilemapControler组件，用于记录分数
    TilemapControllerLocal tilemapcontroller;
    //显示倒计时
    public TMPro.TextMeshProUGUI TimeCount;
    //倒计时
    public float TotalTime;
    private float curTime;
    //分数
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
        //更新分数
        Score = tilemapcontroller.getScore(PlayerColorTag);
        ScoreBarController.ScoreBar.setValue((float)Score / TargetScore);
        //更新倒计时
        curTime = Mathf.Clamp(curTime - Time.deltaTime, 0, TotalTime);
        TimeCount.text = $"Time: {(int)curTime}s";
        //玩家是否死亡
        if (PlayerController.IsDead) {
            NoTimeOrDie(false);
        } else if (Mathf.Approximately(curTime, 0)) { //时间是否耗尽
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
