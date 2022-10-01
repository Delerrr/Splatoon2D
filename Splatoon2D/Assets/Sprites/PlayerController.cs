using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //动画控制器组件
    private Animator animator;
    //是否潜水
    private bool is_diving = false;
    //是否移动
    private bool is_walking = false;
    //移动速度
    public float dive_speed = 10.0f;
    public float walk_speed = 10.0f;
    //方向键输入
    private float horizontal;
    private float vertical;
    //Idle状态方向
    private float standPosX = 0;
    private float standPosY = -1;
    //刚体组件
    Rigidbody2D rigidbody2d;
    //生命值
    public int maxHealth = 100;
    private int currentHealth; 
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Mathf.Approximately(horizontal, 0) && Mathf.Approximately(vertical, 0)) {
            is_walking = false;
            animator.SetBool("IsWalking", is_walking);
            animator.SetFloat("StandPos X", standPosX);
            animator.SetFloat("StandPos Y", standPosY);
        } else {
            is_walking = true;
            animator.SetBool("IsWalking", is_walking);
            standPosX = horizontal < 0 ? -1 : horizontal > 0 ? 1 : 0;
            standPosY = vertical < 0 ? -1 : vertical > 0 ? 1 : 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            is_diving = true;
            animator.SetBool("ISDiving", is_diving);
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            is_diving = false;
            animator.SetBool("ISDiving", is_diving);
        }
    }

    private void FixedUpdate() {
        move();
    }

    private void move() {
        float xPos, yPos;
        if (is_diving) {
            xPos = dive_speed * horizontal * Time.deltaTime;
            yPos = dive_speed * vertical * Time.deltaTime;
        } else {
            xPos = walk_speed * horizontal * Time.deltaTime;
            yPos = walk_speed * vertical * Time.deltaTime;
        }

        if (is_walking) {
            animator.SetFloat("Pos X", horizontal < 0 ? -1 : horizontal > 0 ? 1 : 0);
            animator.SetFloat("Pos Y", vertical < 0 ? -1 : vertical > 0 ? 1 : 0);
        }

        Vector2 position = transform.position;
        position.x += xPos;
        position.y += yPos;
        rigidbody2d.position = position;
    }
}
