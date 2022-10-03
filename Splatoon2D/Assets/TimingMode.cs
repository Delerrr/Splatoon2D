using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingMode : MonoBehaviour
{
    //�����ɫ����int��ʾ�����ڵ���tilemapcontroller.GetScore��
    //0��G��1��B��2��R
    public int PlayerColorTag;
    //TilemapControler��������ڼ�¼����
    TilemapController tilemapcontroller;
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
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
    }


    // Update is called once per frame
    //TODO��
    //���ݷ����򵹼�ʱ��������Ϸ�Ƿ����
    void Update()
    {
        //���·���
        Score = tilemapcontroller.getScore(PlayerColorTag);
        ScoreBarController.ScoreBar.setValue((float)Score / TargetScore);
        //���µ���ʱ
        curTime -= Time.deltaTime;
        TimeCount.text = $"Time: {(int)curTime}s";
    }

    //��Ҫ��Ϊ����PlayerControllerҲ�ܵ��ã�������������Ϸ�Ƿ��ʤֻ��һ���ű���ʵ��
    public static void WinOrLose(bool tag) {
        //��ʵ��
    }
}
