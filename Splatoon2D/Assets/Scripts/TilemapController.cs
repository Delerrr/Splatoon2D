using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{

    //用来调用RPC以更新颜色
    PhotonView photonView;
    public Tilemap worldtilemap;
    public  Color Green;
    public  Color Blue;
    public  Color Red;
    public static int GreenScore = 0;
    public static int BlueScore = 0;
    public static int RedScore = 0;

    private void Start() {
        BlueScore = 0;
        GreenScore = 0;
        RedScore = 0;
        photonView = PhotonView.Get(this);
    }

    private void Update() {
        if (worldtilemap == null) {
            GameObject tempobject = GameObject.FindGameObjectWithTag("Tilemap");
            if (tempobject != null) {
                worldtilemap = tempobject.GetComponent<Tilemap>();
            }
        }
    }

    public void UpdateColor(Vector3Int Pos, Color newColor) {
        int[] temp = { Pos.x, Pos.y, Pos.z };
        float[] tempnewColor = { newColor.r, newColor.g, newColor.b }; 
        photonView.RPC("UpdateColorSync", RpcTarget.All, temp, tempnewColor);
    }


    [PunRPC]
    public void UpdateColorSync(int[] tempPos, float[] tempNewColor) {
        Vector3Int Pos = new(tempPos[0], tempPos[1], tempPos[2]);
        Color newColor = new(tempNewColor[0], tempNewColor[1], tempNewColor[2]);
        if (Colorcmp(worldtilemap.GetColor(Pos), newColor)) {
            return;
        }
        changeScore(FindColor(worldtilemap.GetColor(Pos)), -5);
        changeScore(FindColor(newColor), 5);
        worldtilemap.SetTileFlags(Pos, TileFlags.None);
        worldtilemap.SetColor(Pos, newColor);
    }

// 原始的不能同步的UpdateColor
/*    public void UpdateColor(Vector3Int Pos, Color newColor) {
        if (Colorcmp(worldtilemap.GetColor(Pos), newColor)) {
            return;
        }

        changescore(findcolor(worldtilemap.getcolor(pos)), -5);
        changescore(findcolor(newcolor), 5);
        worldtilemap.settileflags(pos, tileflags.none);
        worldtilemap.setcolor(pos, newcolor);
    }
*/
    public bool Colorcmp(Color one, Color two) {
        return Mathf.Approximately(one.r, two.r) &&
                Mathf.Approximately(one.r, two.r) &&
                Mathf.Approximately(one.r, two.r) &&
                Mathf.Approximately(one.r, two.r);
    }
    //获取分数（0：G， 1：B， 2：R）
    public int getScore(int tag) {
        if (tag == 0) {
            return GreenScore;
        } else if (tag == 1) {
            return BlueScore;
        } else if (tag == 2) {
            return RedScore;
        }
        return -1;
    }
    private void changeScore(string colorname, int amount) {
        if (colorname == "Green") {
            GreenScore = Mathf.Clamp(GreenScore + amount, 0, 9999);
        } else if (colorname == "Blue") {
            BlueScore = Mathf.Clamp(BlueScore + amount, 0, 9999);
        } else if (colorname == "Red") {
            RedScore = Mathf.Clamp(RedScore + amount, 0, 9999);
        }
    }

/*    public void UpdateOpponentScore(string colorname, int newScore) {
        if (colorname == "Green") {
            GreenScore = Mathf.Clamp(newScore, 0, 9999);
        } else if (colorname == "Blue") {
            BlueScore = Mathf.Clamp(newScore, 0, 9999);
        } else if (colorname == "Red") {
            RedScore = Mathf.Clamp(newScore, 0, 9999);
        }
        
    }
*/
    private string FindColor(Color unknown) {
        if (Colorcmp(unknown, Green)) {
            return "Green";
        }
        if (Colorcmp(unknown, Blue)) {
            return "Blue";
        }
        if (Colorcmp(unknown, Red)) {
            return "Red";
        }
        return "Origin";
    }

    public Color GetColorInPos(Vector3Int Pos) {
        return worldtilemap.GetColor(Pos);
    }
    public Vector3Int GetCellPos(Vector3 pos) {
        return worldtilemap.WorldToCell(pos);
    }
}
