using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    //�յĸ����󣬴����ͣ�˵���������ť
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
        //�������ɼ�
        Cursor.visible = true;
        Time.timeScale = 1f;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }

    void ExitGameFunc() {
#if     UNITY_EDITOR//�ڱ༭��ģʽ��
        EditorApplication.isPlaying = false;
#else
        Application.Quit();//�ѷ����汾
#endif
    }
}
