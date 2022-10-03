using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMode : MonoBehaviour
{
    //玩家颜色（用int表示，用于调用tilemapcontroller.GetScore）
    //0：G，1：B，2：R
    public int PlayerColorTag;
    //TilemapControler组件，用于记录分数
    TilemapController tilemapcontroller;
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
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
    }


    // Update is called once per frame
    //TODO：
    //根据分数或倒计时来决定游戏是否结束
    void Update()
    {
        //更新分数
        Score = tilemapcontroller.getScore(PlayerColorTag);
        ScoreBarController.ScoreBar.setValue((float)Score / TargetScore);
        //更新倒计时
        curTime -= Time.deltaTime;
        TimeCount.text = $"Time: {(int)curTime}s";
    }

    //主要是为了让PlayerController也能调用，这样可以让游戏是否获胜只在一个脚本里实现
    public static void WinOrLose(bool tag) {
        //待实现
    }
}
