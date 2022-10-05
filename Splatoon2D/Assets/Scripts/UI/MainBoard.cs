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
    public Button MultiPlayer;
    // Start is called before the first frame update
    void Start() {
        Exit.onClick.AddListener(ExitGame);
        SinglePlayer.onClick.AddListener(EnterSinglePlayer);
        MultiPlayer.onClick.AddListener(EnterMultiPlayer);
    }

    void EnterMultiPlayer() {
        SceneManager.LoadScene("OnlineScene");
    }
    void ExitGame() {
#if     UNITY_EDITOR//在编辑器模式下
        EditorApplication.isPlaying = false;
#else
        Application.Quit();//已发布版本
#endif
    }

    void EnterSinglePlayer() {
        SceneManager.LoadScene("Level_1");
    }
}
