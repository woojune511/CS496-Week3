using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Logic : MonoBehaviour
{
    Transform enemyTransfrom;
    Transform playerTransform;
    Vector2 difference;
    public GameObject bullet;
    public Transform firePos;
    public bool canAttack = true;
    public bool canSpinAttack = true;

    public float coolTime;
    public float spinShotCoolTime = 0.10f;
    public float shotSpeed;
    public Vector2 attackDirection;

    public float rot_Speed = 2f;

    // Start is called before the first frame update
    protected void Start()
    {
        GameManager.instance.isBossDead = false;
        enemyTransfrom = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isStarted)
           {
                //회전
                firePos.Rotate(new Vector3(0, 0, 1) * (rot_Speed*100) * Time.deltaTime);
                if (canAttack)
                {
                    canAttack = false;
                    ShootBullet();
                }
                if (canSpinAttack) 
                {
                    canSpinAttack = false;
                    SpinShootBullet();
                }
            }
    }

    void SetAttackDir() {
        if (playerTransform == null) {
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        Vector2 playerPos = playerTransform.position;
        Vector2 enemyPos = enemyTransfrom.position;
        difference = playerPos - enemyPos;
        Vector2 randomDiff = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        attackDirection = difference + randomDiff;
    }

    void ShootBullet() {
        MakeBullet();
        StartCoroutine(WaitForTargetShot());
    }
    void SpinShootBullet() {
        MakeSpinBullet();
        StartCoroutine(WaitForSpinShot());
    }

    
    void MakeSpinBullet() {
        Bullet tempBullet = Instantiate(bullet, firePos.position, firePos.rotation).GetComponent<Bullet>();
        tempBullet.speed = shotSpeed;
        // SetAttackDir();
        // tempBullet.bulletDir = attackDirection;
        tempBullet.bulletDir = tempBullet.transform.localRotation * Vector2.right;
        tempBullet.popTime = 2f;
    }
    void MakeBullet() {
        Bullet tempBullet = Instantiate(bullet, firePos.position, transform.rotation).GetComponent<Bullet>();
        tempBullet.speed = shotSpeed;
        SetAttackDir();
        tempBullet.bulletDir = attackDirection;
        tempBullet.popTime = 10f;
    }

    IEnumerator WaitForTargetShot()
    {
        yield return new WaitForSeconds(coolTime);
        canAttack = true;
    }
    IEnumerator WaitForSpinShot()
    {
        yield return new WaitForSeconds(spinShotCoolTime);
        canSpinAttack = true;
    }

}
