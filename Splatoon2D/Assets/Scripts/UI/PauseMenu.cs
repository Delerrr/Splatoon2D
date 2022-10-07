using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    //空的父对象，存放暂停菜单的三个按钮
    public GameObject pausemenu;
    public Button Pause;
    public Button Resume;
    public Button BackToMain; 
    public Button ExitGame; 

    // Start is called before the first frame update
    void Start()
    {
        Pause.onClick.AddListener(PauseGame);
        Resume.onClick.AddListener(ResumeGame);
        BackToMain.onClick.AddListener(BackToMainMenu);
        ExitGame.onClick.AddListener(ExitGameFunc);
    }

    void PauseGame() {
        Time.timeScale = 0;
        pausemenu.SetActive(true);
    }

    void ResumeGame() {
        Time.timeScale = 1f;
        pausemenu.SetActive(false);
    }

    void BackToMainMenu() {
        //设置鼠标可见
        Cursor.visible = true;
        Time.timeScale = 1f;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    void ExitGameFunc() {
#if     UNITY_EDITOR//在编辑器模式下
        EditorApplication.isPlaying = false;
#else
        Application.Quit();//已发布版本
#endif
    }
}
