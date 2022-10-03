using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBoxController : MonoBehaviour
{
    //�˺�ֵ
    public float harm = 10;
    //�ٶ�
    public float speed = 10.0f;
    //��ʼλ��
    private float oriPosx;
    private float oriPosy;
    //ˮƽ�߻�����ֱ��
    public bool walkHorizontally = true;
    public float halfCycle = 3;
    //oriDrection = 1 ��ǰ�����������/���ߣ�-1 ������/����
    public int oriDirection = 1;
    //ÿ���������ѹ�ȥ��ʱ��
    private float timePassed = 0;
    //����ֵ
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
                } else {
                    ChangeRigidPosition(0f, oriDirection * Time.deltaTime * speed);
                }
            } else if (timePassed < 2 * halfCycle) {
                timePassed += Time.deltaTime;
                if (walkHorizontally) {
                    ChangeRigidPosition(-oriDirection * Time.deltaTime * speed, 0f);
                } else {
                    ChangeRigidPosition(0f, -oriDirection * Time.deltaTime * speed);
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
        LaserBulletController laserbulletcontroller = other.gameObject.GetComponent<LaserBulletController>(); 
        if (laserbulletcontroller != null) {
            ChangeHealth(-laserbulletcontroller.HarmAmount);
            return;
        }
        SmallBulletController smallbulletcontroller = other.gameObject.GetComponent<SmallBulletController>();
        if (smallbulletcontroller != null) {
            ChangeHealth(-smallbulletcontroller.HarmAmount);
        }
    }
}
