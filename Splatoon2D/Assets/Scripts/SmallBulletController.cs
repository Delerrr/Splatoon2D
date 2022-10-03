using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SmallBulletController : MonoBehaviour
{
    //伤害值
    public float HarmAmount = 5;
    //是否已经发射，用于决定update中是否进行染色
    bool IsLaunched = false;
    //TilemapController组件，用于涂色
    TilemapController tilemapcontroller;
    //飞行时间
    public float FlyTime = 0.5f;
    private float TimeFlew = 0f;
    //颜色
    public Color Bulletcolor;
    //force影响子弹速度
    public float force = 300;
    //获取刚体组件
    private Rigidbody2D rigidbody2d;
    void Awake()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
    }

    private void Update() {
        if (!IsLaunched) {
            return;
        }
        TimeFlew += Time.deltaTime;
        //子弹消失时开始染色
        if (TimeFlew >= FlyTime) {
                Vector3Int tilePosition = tilemapcontroller.GetCellPos(transform.position);
                Color newcolor = new Color(Bulletcolor.r, Bulletcolor.g, Bulletcolor.b);
                tilemapcontroller.UpdateColor(tilePosition, newcolor);
                tilePosition.x -= 1;
                tilemapcontroller.UpdateColor(tilePosition, newcolor);
                tilePosition.x += 2;
                tilemapcontroller.UpdateColor(tilePosition, newcolor);
                tilePosition.x -= 1;
                tilePosition.y -= 1;
                tilemapcontroller.UpdateColor(tilePosition, newcolor);
                tilePosition.y += 2;
                tilemapcontroller.UpdateColor(tilePosition, newcolor);
            Destroy(gameObject);
            return;
        }
    }

    //clienttag: 0:玩家，1：敌人
    public void Launch(TilemapController controller, int clienttag, Vector2 clientpos) {
        tilemapcontroller = controller;
        IsLaunched = true;
        if (clienttag == 0) {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Pos.x -= transform.position.x;
            Pos.y -= transform.position.y;
            float RotateDegree = Mathf.Rad2Deg * Mathf.Atan2(Pos.y, Pos.x);
            //旋转至指向鼠标方向
            transform.Rotate(0, 0, RotateDegree);
            Pos.Normalize();
            rigidbody2d.AddForce(Pos * force);
        } else if (clienttag == 1) {
            rigidbody2d.AddForce(clientpos * force);
        } 
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
        PlayerController playercontroller = other.gameObject.GetComponent<PlayerController>();
        if (playercontroller != null) {
            Vector2 otherPosition = other.transform.position;
            Vector2 position = transform.position;
            otherPosition.x -= position.x;
            otherPosition.y -= position.y;
            otherPosition.Normalize();
            playercontroller.Attackted(otherPosition, HarmAmount);
            return; 
        }
    }
}
