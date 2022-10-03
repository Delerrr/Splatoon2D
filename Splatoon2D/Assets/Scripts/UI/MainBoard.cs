using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainBoard : MonoBehaviour
{
    public Button SinglePlayer;
    public Button Exit;
    // Start is called before the first frame update
    void Start() {
        Exit.onClick.AddListener(ExitGame);
        SinglePlayer.onClick.AddListener(EnterSinglePlayer);
    }

    void ExitGame() {
#if     UNITY_EDITOR//�ڱ༭��ģʽ��
        EditorApplication.isPlaying = false;
#else
        Application.Quit();//�ѷ����汾
#endif
    }

    void EnterSinglePlayer() {
        SceneManager.LoadScene("Level_1");
    }
}
