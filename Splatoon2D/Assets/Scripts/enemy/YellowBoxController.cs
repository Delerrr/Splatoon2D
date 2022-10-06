using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBoxController : MonoBehaviour
{
    //颜色 
    public Color BoxColor;
    //TilemapControler组件，用于染色
    TilemapControllerLocal tilemapcontroller;
    //伤害值
    public float harm = 10;
    //速度
    public float speed = 10.0f;
    //初始位置
    private float oriPosx;
    private float oriPosy;
    //水平走还是竖直走
    public bool walkHorizontally = true;
    public float halfCycle = 3;
    //oriDrection = 1 则前半个周期向右/上走，-1 则向左/下走
    public int oriDirection = 1;
    //每个周期中已过去的时间
    private float timePassed = 0;
    //生命值
    public float maxHealth = 25;
    protected float currentHealth;

    protected Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    protected void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        maxHealth = 15;
        Vector2 position = transform.position;
        oriPosx = position.x;
        oriPosy = position.y;
        rigidbody2d.isKinematic = false;
        tilemapcontroller = gameObject.GetComponent<TilemapControllerLocal>();
    }

    protected void ChangeRigidPosition(float xPos, float yPos) {
        Vector2 position = transform.position;
        position.x += xPos;
        position.y += yPos;
        rigidbody2d.position = position;
    }

    protected void ChangePosition(float xPos, float yPos) {
        Vector2 position = transform.position;
        position.x += xPos;
        position.y += yPos;
        transform.position = position;
    }


    public float Health
    {
        get
        {
            return currentHealth;
        }
    }

    public void ChangeHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (Mathf.Approximately(currentHealth, 0)) {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate() {
            if (timePassed < halfCycle) {
                timePassed += Time.deltaTime;
                if (walkHorizontally) {
                    ChangeRigidPosition(oriDirection * Time.deltaTime * speed, 0f);
                    Vector3Int tilePosition = tilemapcontroller.GetCellPos(transform.position);
                    tilemapcontroller.UpdateColor(tilePosition, BoxColor);
                } else {
                    ChangeRigidPosition(0f, oriDirection * Time.deltaTime * speed);
                    Vector3Int tilePosition = tilemapcontroller.GetCellPos(transform.position);
                    tilemapcontroller.UpdateColor(tilePosition, BoxColor);
                }
            } else if (timePassed < 2 * halfCycle) {
                timePassed += Time.deltaTime;
                if (walkHorizontally) {
                    ChangeRigidPosition(-oriDirection * Time.deltaTime * speed, 0f);
                    Vector3Int tilePosition = tilemapcontroller.GetCellPos(transform.position);
                    tilemapcontroller.UpdateColor(tilePosition, BoxColor);
                } else {
                    ChangeRigidPosition(0f, -oriDirection * Time.deltaTime * speed);
                    Vector3Int tilePosition = tilemapcontroller.GetCellPos(transform.position);
                    tilemapcontroller.UpdateColor(tilePosition, BoxColor);
                }
            } else {
                timePassed = 0;
                Vector2 position = transform.position;
                position.x = oriPosx;
                position.y = oriPosy;
            }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        PlayerController playercontroller = other.gameObject.GetComponent<PlayerController>();
        if (playercontroller != null) {
            Vector2 otherPosition = other.transform.position;
            Vector2 position = transform.position;
            otherPosition.x -= position.x;
            otherPosition.y -= position.y;
            otherPosition.Normalize();
            playercontroller.Attackted(otherPosition, harm);
            return; 
        }
        LaserBulletControllerLocal laserbulletcontroller = other.gameObject.GetComponent<LaserBulletControllerLocal>(); 
        if (laserbulletcontroller != null) {
            ChangeHealth(-laserbulletcontroller.HarmAmount);
            return;
        }
        SmallBulletControllerLocal smallbulletcontroller = other.gameObject.GetComponent<SmallBulletControllerLocal>();
        if (smallbulletcontroller != null) {
            ChangeHealth(-smallbulletcontroller.HarmAmount);
        }
    }
}
