using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LaserBulletController : MonoBehaviour
{
    //是否已经发射，用于决定update中是否进行染色
    bool IsLaunched = false;

    //Tilemap组件，用于涂色
    Tilemap worldtilemap;
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
    }

    private void Update() {
        if (IsLaunched == false) {
            return;
        }
        TimeFlew += Time.deltaTime;
        Vector3Int tilePosition = worldtilemap.WorldToCell(transform.position);
        if (worldtilemap.HasTile(tilePosition)) {
            worldtilemap.SetTileFlags(tilePosition, TileFlags.None);
            Color newcolor = new Color(Bulletcolor.r, Bulletcolor.g, Bulletcolor.b);
            worldtilemap.SetColor(tilePosition, newcolor);
            tilePosition.x -= 1;
            worldtilemap.SetTileFlags(tilePosition, TileFlags.None);
            worldtilemap.SetColor(tilePosition, newcolor);
            tilePosition.x += 2;
            worldtilemap.SetTileFlags(tilePosition, TileFlags.None);
            worldtilemap.SetColor(tilePosition, newcolor);
        }
        //销毁
        if (TimeFlew >= FlyTime) {
            Destroy(gameObject);
            return;
        }
    }
    public void Launch(Tilemap tilemap) {
        IsLaunched = true;
        worldtilemap = tilemap;
        Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Pos.x -= transform.position.x;
        Pos.y -= transform.position.y;
        float RotateDegree = Mathf.Rad2Deg * Mathf.Atan2(Pos.y, Pos.x);
        //旋转至指向鼠标方向
        transform.Rotate(0, 0, RotateDegree);
        Pos.Normalize();
        rigidbody2d.AddForce(Pos * force);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
