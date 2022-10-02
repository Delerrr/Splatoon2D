using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //�����ɫ����int��ʾ�����ڵ���tilemapcontroller.GetScore��
    public int PlayerColorTag;
    //��UI����ʾ����
    public TMPro.TextMeshProUGUI ScoreText;
    //����
    private int Score = 0;
    //�ӵ�����ʱ�������ҵ�λ��
    private Vector3[,] BulletLauncPos;
    //TilemapControler��������ڼ�¼����
    TilemapController tilemapcontroller;
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
    public float maxHealth = 100;
    private float currentHealth; 
    //īˮֵ
    public float maxInk = 20;
    private float currentInk;
    //ÿ��ظ���īˮ��
    public float inkIncreaseSpeed = 5;
    //����������īˮ����
    public float smallBulletInkConsume = 2;
    public float laserBulletInkConsume = 8;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        tilemapcontroller = gameObject.GetComponent<TilemapController>();
        currentHealth = maxHealth;
        currentInk = maxInk;
        animator = GetComponent<Animator>();
        //�ӵ�����ʱ��λ�ã��������ң�
        BulletLauncPos = new Vector3[2, 4];
        BulletLauncPos[0, 0] = new Vector3(-0.08f, 1.864f);
        BulletLauncPos[0, 1] = new Vector3(-0.376f, -0.009f);
        BulletLauncPos[0, 2] = new Vector3(-1.025f, 0.756f);
        BulletLauncPos[0, 3] = new Vector3(0.180f, 0.852f);
        BulletLauncPos[1, 0] = new Vector3(0f, 2.084f);
        BulletLauncPos[1, 1] = new Vector3(-0.356f, -0.152f);
        BulletLauncPos[1, 2] = new Vector3(-1.417f, 0.784f);
        BulletLauncPos[1, 3] = new Vector3(0.752f, 0.708f);

        //ǹ�����ַ���
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
        //���·���
        ScoreText.text = $"Score: {tilemapcontroller.getScore(PlayerColorTag)}";
        //�����ӵ�
        if (Input.GetMouseButtonDown(0) && !is_diving && !Mathf.Approximately(currentInk, 0)) {
            GameObject bullet = Instantiate(bullets[weapontag1], transform.position + BulletLauncPos[weapontag1, weapontag2], Quaternion.identity);
            if (weapontag1 == 0) {
                SmallBulletController bulletscript = bullet.GetComponent<SmallBulletController>();
                bulletscript.Launch(tilemapcontroller);
                ChangeInk(-smallBulletInkConsume);
            } else {
                LaserBulletController bulletscript = bullet.GetComponent<LaserBulletController>();
                bulletscript.Launch(tilemapcontroller);
                ChangeInk(-laserBulletInkConsume);
            }
        }
        //�л�����
        SwitchWeapon();
        //��ȡ�����
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //�Ƿ�Ǳˮ
        if (Input.GetKey(KeyCode.LeftShift)) {
            Vector3Int tilePosition = tilemapcontroller.GetCellPos(transform.position);
            Color tilecolor = tilemapcontroller.GetColorInPos(tilePosition);
            if (Mathf.Approximately(tilecolor.r, playercolor.r) &&
                Mathf.Approximately(tilecolor.g, playercolor.g) &&
                Mathf.Approximately(tilecolor.b, playercolor.b)) {
                is_diving = true;
                animator.SetBool("IsDiving", is_diving);
                is_walking = false;
            } else {
                is_diving = false;
            }
            animator.SetBool("IsDiving", is_diving);
        } else {
            is_diving = false;
            animator.SetBool("IsDiving", is_diving);
        }
        //���Ǳˮ, ��һ���ظ�īˮ
        if (is_diving) {
            //�ظ�īˮ
            ChangeInk(inkIncreaseSpeed * Time.deltaTime);
            weapon.SetActive(false);
            //���ƶ�
            if (!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0)) {
                standPosX = horizontal < 0 ? -1 : horizontal > 0 ? 1 : 0;
                standPosY = vertical < 0 ? -1 : vertical > 0 ? 1 : 0;
                animator.SetFloat("Pos X", standPosX);
                animator.SetFloat("Pos Y", standPosY);
            }
        } else {
            //�з�������룬��walk
            if (!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0)) {
                is_walking = true;
                standPosX = horizontal < 0 ? -1 : horizontal > 0 ? 1 : 0;
                standPosY = vertical < 0 ? -1 : vertical > 0 ? 1 : 0;
                animator.SetFloat("Pos X", standPosX);
                animator.SetFloat("Pos Y", standPosY);
            } else {
                is_walking = false;
            }
            animator.SetBool("IsWalking", is_walking);
        }
    }

    private void FixedUpdate() {
        move();
    }

    //��ӡ����
    void ShowScore() {
        ScoreText.text = "Score:\t" + Score;
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

    public void ChangeHealth(float amount) {
        currentHealth = Mathf.Clamp(amount + currentHealth, 0, maxHealth);
        HealthBarController.HealthBar.setValue(currentHealth / maxHealth);
    }

    private void ChangeInk(float amount) {
        currentInk = Mathf.Clamp(amount + currentInk, 0, maxInk);
        InkBarController.InkBar.setValue(currentInk / maxInk);
    }
}
