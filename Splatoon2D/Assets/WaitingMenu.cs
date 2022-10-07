using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingMenu : MonoBehaviour
{
    public Button BackToMain;

    // Start is called before the first frame update
    void Start() {
        BackToMain.onClick.AddListener(BackToMainMenu);
    }



    void BackToMainMenu() {
        //�������ɼ�
        Cursor.visible = true;
        Time.timeScale = 1f;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}

