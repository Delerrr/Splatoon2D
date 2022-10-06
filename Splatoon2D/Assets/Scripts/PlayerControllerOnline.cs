using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerControllerOnline: MonoBehaviour
{
    //����ͬ�����
    PhotonView photonview; 
    //�Ƿ�������������ʾ��ؽ���
    public static bool GIsDead = false;
    public static bool RIsDead = false;
    //�ӵ�����ʱ�������ҵ�λ��
    private Vector3[,] BulletLauncPos;
    //TilemapControler��������ڷ����ӵ�����������Ⱦɫ��
    TilemapController tilemapcontroller;
    //����
    private int score = 0;
    //�ӵ�Ԥ�Ƽ�
    public GameObject[] bullets;
    //��ɫ
    public Color playercolor;
    //��ɫ����0��1��2��ʾ��
    public int ColorTag;
    //��ǰ��������
    private GameObject weapon;
    //������������Ҷǹ(1)��Զ������Ϳǹ���ҳ�֮Ϊlaser��(2)��ը��(3)
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
    public float low_speed = 0.1f;
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
    //�ڵ���īˮ��ÿ��ʧȥ������ֵ
    public float HealthDecreaseInOtherInkPerSec = 15f;
    private float HealthDecreaseTimePassed = 0;
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
        tilemapcontroller = NetworkManager.tilemapcontroller;
        photonview = gameObject.GetComponent<PhotonView>();
        rigidbody2d = GetComponent<Rigidbody2D>();
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

/*    public void SetOpponetScore(string colorname, int Score) {
        tilemapcontroller.UpdateOpponentScore(colorname, Score);
    }
*/    public float GetScore() {
        return score;
    }
    void Update() {

        if (!photonview.IsMine) return;
/*        if (ColorTag == 0) {
            score = TilemapController.GreenScore;
        } else {
            score = TilemapController.RedScore;
        }
        //������ǵ�ǰ�ͻ��������·������Է�
        if (!photonview.IsMine) {
            if (ColorTag == 0) {
                NetworkManager.UpdateOpponentScore("Green", score);
            } else {
                NetworkManager.UpdateOpponentScore("Red", score);
            }
            return;
        }
*/
        //�����ӵ�
        if (Input.GetMouseButtonDown(0) && !is_diving && !Mathf.Approximately(currentInk, 0)) {
            GameObject bullet = NetworkManager.InstanstiateAndLaunch(bullets[weapontag1].name, transform.position + BulletLauncPos[weapontag1, weapontag2], Quaternion.identity);
            if (weapontag1 == 0) {
                SmallBulletController bulletscript = bullet.GetComponent<SmallBulletController>();
                bulletscript.Launch(tilemapcontroller, 0, transform.position);
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
            if (tilemapcontroller.Colorcmp(tilecolor, playercolor)){ 
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

    //������
    public void Attackted(Vector2 Pos, float HarmAmount) {
        animator.SetTrigger("Hit");
        animator.SetFloat("HitPosX", Pos.x);
        animator.SetFloat("HitPosY", Pos.y);
        ChangeHealth(-HarmAmount);
    }
    private void FixedUpdate() {
        if (!photonview.IsMine) return;
        move();
        //�Ƿ��ڵ��˵�īˮ��
        if (IsInOtherInk()){ 
            //�ڵ��˵�īˮ��
            if (HealthDecreaseTimePassed >= 1f) {
                ChangeHealth(-HealthDecreaseInOtherInkPerSec);
                HealthDecreaseTimePassed = 0;
            } else {
                HealthDecreaseTimePassed += Time.deltaTime;
            }
        }
    }

    private bool IsInOtherInk() {
        Vector3Int tilePosition = tilemapcontroller.GetCellPos(transform.position);
        Color tilecolor = tilemapcontroller.GetColorInPos(tilePosition);
        return !tilemapcontroller.Colorcmp(playercolor, tilecolor) && !tilemapcontroller.Colorcmp(new Color(1, 1, 1, 1), tilecolor);
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
        } else if (IsInOtherInk()) {
            xPos = low_speed * horizontal * Time.deltaTime;
            yPos = low_speed * vertical * Time.deltaTime;
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
        if (Mathf.Approximately(currentHealth, 0)) {
            if (Mathf.Approximately(playercolor.r, 52f / 255)) {
                GIsDead = true;
            } else {
                RIsDead = true;
            }
        }
    }

    private void ChangeInk(float amount) {
        currentInk = Mathf.Clamp(amount + currentInk, 0, maxInk);
        InkBarController.InkBar.setValue(currentInk / maxInk);
    }
}
