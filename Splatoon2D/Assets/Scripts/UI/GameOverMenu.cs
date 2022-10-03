using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{

    //空的父对象，存放暂停菜单的三个按钮
    public GameObject gameovermenu;
    public Button TryAgain;
    public Button BackToMain; 
    public Button ExitGame; 

    void Start()
    {
        TryAgain.onClick.AddListener(TryAgainGame);
        BackToMain.onClick.AddListener(BackToMainMenu);
        ExitGame.onClick.AddListener(ExitGameFunc);
    }

    void TryAgainGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void BackToMainMenu() {
        //设置鼠标可见
        Cursor.visible = true;
        Time.timeScale = 1f;
        gameovermenu.SetActive(false);
        SceneManager.LoadScene("MainBoard");
    }

    void ExitGameFunc() {
#if     UNITY_EDITOR//在编辑器模式下
        EditorApplication.isPlaying = false;
#else
        Application.Quit();//已发布版本
#endif
    }
}
