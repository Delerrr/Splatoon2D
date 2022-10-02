using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LaserBulletController : MonoBehaviour
{
    //Tilemap���������Ϳɫ
    public Tilemap worldtilemap;
    //����ʱ��
    public float FlyTime = 0.5f;
    private float TimeFlew = 0f;
    //��ɫ
    public Color Bulletcolor;
    //forceӰ���ӵ��ٶ�
    public float force = 300;
    //��ȡ�������
    private Rigidbody2D rigidbody2d;
    void Awake()
    {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update() {
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
        //����
        if (TimeFlew >= FlyTime) {
            Destroy(gameObject);
            return;
        }
    }
    public void Launch(Tilemap tilemap) {
        worldtilemap = tilemap;
        Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Pos.x -= transform.position.x;
        Pos.y -= transform.position.y;
        float RotateDegree = Mathf.Rad2Deg * Mathf.Atan2(Pos.y, Pos.x);
        //��ת��ָ����귽��
        transform.Rotate(0, 0, RotateDegree);
        Pos.Normalize();
        rigidbody2d.AddForce(Pos * force);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}