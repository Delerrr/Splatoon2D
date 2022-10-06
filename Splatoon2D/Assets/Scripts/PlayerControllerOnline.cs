using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class PlayerControllerOnline: MonoBehaviour
{
    //联网同步组件
    PhotonView photonview; 
    //是否死亡，用于显示相关界面
    public static bool GIsDead = false;
    public static bool RIsDead = false;
    //子弹发射时相对于玩家的位置
    private Vector3[,] BulletLauncPos;
    //TilemapControler组件，用于发射子弹（最终用于染色）
    TilemapController tilemapcontroller;
    //分数
    private int score = 0;
    //子弹预制件
    public GameObject[] bullets;
    //颜色
    public Color playercolor;
    //颜色（用0、1、2表示）
    public int ColorTag;
    //当前所持武器
    private GameObject weapon;
    //三种武器：新叶枪(1)、远距离喷涂枪（我称之为laser）(2)、炸弹(3)
    private int weapontag1 = 0;
    private int weapontag2 = 1;
    private string[,] weaponname;
    //动画控制器组件
    private Animator animator;
    //是否潜水
    private bool is_diving = false;
    //是否移动
    private bool is_walking = false;
    //移动速度
    public float dive_speed = 10.0f;
    public float walk_speed = 10.0f;
    public float low_speed = 0.1f;
    //方向键输入
    private float horizontal;
    private float vertical;
    //Idle状态方向
    private float standPosX = 0;
    private float standPosY = -1;
    //刚体组件
    Rigidbody2D rigidbody2d;
    //生命值
    public float maxHealth = 100;
    private float currentHealth;
    //在敌人墨水中每秒失去的生命值
    public float HealthDecreaseInOtherInkPerSec = 15f;
    private float HealthDecreaseTimePassed = 0;
    //墨水值
    public float maxInk = 20;
    private float currentInk;
    //每秒回复的墨水量
    public float inkIncreaseSpeed = 5;
    //各武器消耗墨水的量
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
        //子弹发射时的位置（相对于玩家）
        BulletLauncPos = new Vector3[2, 4];
        BulletLauncPos[0, 0] = new Vector3(-0.08f, 1.864f);
        BulletLauncPos[0, 1] = new Vector3(-0.376f, -0.009f);
        BulletLauncPos[0, 2] = new Vector3(-1.025f, 0.756f);
        BulletLauncPos[0, 3] = new Vector3(0.180f, 0.852f);
        BulletLauncPos[1, 0] = new Vector3(0f, 2.084f);
        BulletLauncPos[1, 1] = new Vector3(-0.356f, -0.152f);
        BulletLauncPos[1, 2] = new Vector3(-1.417f, 0.784f);
        BulletLauncPos[1, 3] = new Vector3(0.752f, 0.708f);

        //枪的四种方向
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
        //如果不是当前客户机，更新分数给对方
        if (!photonview.IsMine) {
            if (ColorTag == 0) {
                NetworkManager.UpdateOpponentScore("Green", score);
            } else {
                NetworkManager.UpdateOpponentScore("Red", score);
            }
            return;
        }
*/
        //发射子弹
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
        //切换武器
        SwitchWeapon();
        //获取方向键
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //是否潜水
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
        //如果潜水, 则一定回复墨水
        if (is_diving) {
            //回复墨水
            ChangeInk(inkIncreaseSpeed * Time.deltaTime);
            weapon.SetActive(false);
            //有移动
            if (!Mathf.Approximately(horizontal, 0) || !Mathf.Approximately(vertical, 0)) {
                standPosX = horizontal < 0 ? -1 : horizontal > 0 ? 1 : 0;
                standPosY = vertical < 0 ? -1 : vertical > 0 ? 1 : 0;
                animator.SetFloat("Pos X", standPosX);
                animator.SetFloat("Pos Y", standPosY);
            }
        } else {
            //有方向键输入，则walk
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

    //被攻击
    public void Attackted(Vector2 Pos, float HarmAmount) {
        animator.SetTrigger("Hit");
        animator.SetFloat("HitPosX", Pos.x);
        animator.SetFloat("HitPosY", Pos.y);
        ChangeHealth(-HarmAmount);
    }
    private void FixedUpdate() {
        if (!photonview.IsMine) return;
        move();
        //是否在敌人的墨水中
        if (IsInOtherInk()){ 
            //在敌人的墨水中
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
        //根据移动方向切换武器方向
        if (standPosX == 0) {
            weapontag2 = (1 - (int)standPosY) / 2;
        } else {
            weapontag2 = ((int)standPosX - 1) / 2 + 3;
        }
        weapon.SetActive(false);
        ShowWeapon();
    }
    //显示出武器，通过设置gameObject.renderer.enabled来实现
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
