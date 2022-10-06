using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LaserBulletControllerLocal : MonoBehaviour
{
    //�˺�ֵ
    public float HarmAmount = 10f;
    //�Ƿ��Ѿ����䣬���ھ���update���Ƿ����Ⱦɫ
    bool IsLaunched = false;

    //TilemapController���������Ϳɫ
    TilemapControllerLocal tilemapcontroller;
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
        if (IsLaunched == false) {
            return;
        }
        TimeFlew += Time.deltaTime;
        Vector3Int tilePosition = tilemapcontroller.GetCellPos(transform.position);
            Color newcolor = new Color(Bulletcolor.r, Bulletcolor.g, Bulletcolor.b);
            tilemapcontroller.UpdateColor(tilePosition, newcolor);
            tilePosition.x -= 1;
            tilemapcontroller.UpdateColor(tilePosition, newcolor);
            tilePosition.x += 2;
            tilemapcontroller.UpdateColor(tilePosition, newcolor);
        //����
        if (TimeFlew >= FlyTime) {
            Destroy(gameObject);
            return;
        }
    }
    public void Launch(TilemapControllerLocal controller) {
        tilemapcontroller = controller;
        IsLaunched = true;
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
