using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public Button NextLevel;
    public Button BackToMain; 

    void Start()
    {
        BackToMain.onClick.AddListener(BackToMainMenu);
        NextLevel.onClick.AddListener(NextLevelFunc);
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
