using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //Tilemap���������Ϳɫ
    public Tilemap worldtilemap;
    //�ӵ�Ԥ�Ƽ�
    public GameObject[] bullets;
    //��ɫ
    public Color playercolor;
    //��ǰ��������
    private GameObject weapon;
    //����������ǹ(1)������ǹ(2)��ը��(3)
    private int weapontag1 = 0;
    private int weapontag2 = 1;
    private string[,] weaponname;
    //�������������
    private Animator animator;
    //�Ƿ�Ǳˮ
    private bool is_diving = false;
    //�Ƿ��ƶ�
    private bool is_walking = false;
    //�ƶ��ٶ�
    public float dive_speed = 10.0f;
    public float walk_speed = 10.0f;
    //���������
    private float horizontal;
    private float vertical;
    //Idle״̬����
    private float standPosX = 0;
    private float standPosY = -1;
    //�������
    Rigidbody2D rigidbody2d;
    //����ֵ
    public int maxHealth = 100;
    private int currentHealth; 
    // Start is called before the first frame update
    void Start()
    {

        playercolor = new Color(52, 204, 45);
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        weaponname = new string[3,4];

        weaponname[0,0] = "gun_up";
        weaponname[0,1] = "gun_down";
        weaponname[0,2] = "gun_side";
        weaponname[0,3] = "gun_sideright";

        weaponname[1,0] = "laser_up";
        weaponname[1,1] = "laser_down";
        weaponname[1,2] = "laser_side";
        weaponname[1,3] = "laser_sideright";

        weaponname[2,0] = "bomb_up";
        weaponname[2,1] = "bomb_down";
        weaponname[2,2] = "bomb_side";
        weaponname[2,3] = "bomb_sideright";

        foreach (Transform eachChild in transform) {
            if (eachChild.name == weaponname[weapontag1, weapontag2]) {
                eachChild.gameObject.SetActive(true);
                weapon = eachChild.gameObject;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�����ӵ�
        if (Input.GetMouseButtonDown(0)) {
            GameObject bullet = Instantiate(bullets[weapontag1], rigidbody2d.position + Vector2.up * 2f, Quaternion.identity);
            BulletController bulletscript = bullet.GetComponent<BulletController>();
            bulletscript.Launch(worldtilemap);
        }
        //�л�����
        SwitchWeapon();
        //��ȡ�����
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //�Ƿ�Ǳˮ
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            is_diving = true;
            animator.SetBool("IsDiving", is_diving);
            is_walking = false;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            is_diving = false;
            animator.SetBool("IsDiving", is_diving);
        }
        //�з�������룬��ҪôwalkҪôdive
        if (!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0)) {
            //Ǳˮ
            if (is_diving) {
                weapon.SetActive(false);
            } else {
                is_walking = true;
                animator.SetBool("IsWalking", is_walking);
            }
            standPosX = horizontal < 0 ? -1 : horizontal > 0 ? 1 : 0;
            standPosY = vertical < 0 ? -1 : vertical > 0 ? 1 : 0;
            animator.SetFloat("Pos X", standPosX);
            animator.SetFloat("Pos Y", standPosY);
        } else {
            is_walking = false;
            animator.SetBool("IsWalking", is_walking);
            //���ƶ�������Ϣ��������������
            animator.SetFloat("StandPos X", standPosX);
            animator.SetFloat("StandPos Y", standPosY);
        }
    }

    private void FixedUpdate() {
        move();
    }

    void SwitchWeapon() {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapontag1 != 0) {
                weapontag1 = 0;
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && weapontag1 != 1) {
                weapontag1 = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha3) && weapontag1 != 2) {
                weapontag1 = 2;
        }
        //�����ƶ������л���������
        if (standPosX == 0) {
            weapontag2 = (1 - (int)standPosY) / 2;
        } else {
            weapontag2 = ((int)standPosX - 1) / 2 + 3;
        }
        weapon.SetActive(false);
        ShowWeapon();
    }
    //��ʾ��������ͨ������gameObject.renderer.enabled��ʵ��
    void ShowWeapon() {
        foreach (Transform eachChild in transform) {
            if (eachChild.name == weaponname[weapontag1, weapontag2]) {
                eachChild.gameObject.SetActive(true);
                weapon = eachChild.gameObject;
                break;
            }
        }
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
