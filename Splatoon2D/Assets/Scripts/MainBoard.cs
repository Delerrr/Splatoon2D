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
