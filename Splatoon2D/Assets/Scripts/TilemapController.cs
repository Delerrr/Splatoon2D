using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    public Tilemap worldtilemap;
    public  Color Green;
    public  Color Blue;
    public  Color Red;
    private static int GreenScore = 0;
    private static int BlueScore = 0;
    private static int RedScore = 0;

    private void Start() {
        BlueScore = 0;
        GreenScore = 0;
        RedScore = 0;
    }
    public void UpdateColor(Vector3Int Pos, Color newColor) {
        if (Colorcmp(worldtilemap.GetColor(Pos), newColor)) {
            return;
        }

        changeScore(FindColor(worldtilemap.GetColor(Pos)), -5);
        changeScore(FindColor(newColor), 5);
        worldtilemap.SetTileFlags(Pos, TileFlags.None);
        worldtilemap.SetColor(Pos, newColor);
    }

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
        } else if (colorname == "red") {
            RedScore = Mathf.Clamp(RedScore + amount, 0, 9999);
        }
    }

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
