using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public Button NextLevel;
    public Button BackToMain;
    private TilemapController tilemapcontroller;

    void Start()
    {
        BackToMain.onClick.AddListener(BackToMainMenu);
        NextLevel.onClick.AddListener(NextLevelFunc);
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
    }

    void NextLevelFunc() {
        SceneManager.LoadScene(2);
    }

    void BackToMainMenu() {
        //�������ɼ�
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainBoard");
    }

}
