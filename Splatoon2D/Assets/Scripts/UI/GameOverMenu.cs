using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{

    //�յĸ����󣬴����ͣ�˵���������ť
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
        //�������ɼ�
        Cursor.visible = true;
        Time.timeScale = 1f;
        gameovermenu.SetActive(false);
        SceneManager.LoadScene("MainBoard");
    }

    void ExitGameFunc() {
#if     UNITY_EDITOR//�ڱ༭��ģʽ��
        EditorApplication.isPlaying = false;
#else
        Application.Quit();//�ѷ����汾
#endif
    }
}
