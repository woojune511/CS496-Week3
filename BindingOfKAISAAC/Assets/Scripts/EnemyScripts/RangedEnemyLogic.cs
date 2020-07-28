using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyLogic : MovingObject
{
    Transform enemyTransfrom;
    Transform playerTransform;
    Vector2 difference;
    public GameObject bullet;
    public Transform firePos;
    public bool canAttack = true;

    public float coolTime = 0.25f;
    public float shotSpeed;
    public Vector2 attackDirection;

    // Start is called before the first frame update
    protected override void Start()
    {
        enemyTransfrom = GetComponent<Transform>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isStarted)
           {
               SetVelocity();
                if (canAttack)
                {
                    canAttack = false;
                    ShootBullet();
                }
            }
    }

    protected override Vector2 GetVector() {

        // 이 두 줄은 나중에 스포너 형식으로 바꾸면 지워도 될 듯
        if(!GameManager.instance.isStarted)
            return Vector2.zero;

        if (playerTransform == null) {
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        Vector2 playerPos = playerTransform.position;
        Vector2 enemyPos = enemyTransfrom.position;
        difference = playerPos - enemyPos;
        // 너무 멀면 가까워지기
        if(difference.magnitude > 5.5)
            return difference;
        // 가까운 상태에선 멀어지기
        return -difference;
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Bullet") {
            base.KnockBack(other.attachedRigidbody.velocity);
        }
    }

    void SetAttackDir() {
        Vector2 randomDiff = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        attackDirection = difference + randomDiff;
    }

    void ShootBullet() {
        MakeBullet();
        StartCoroutine(WaitForIt());
    }

    void MakeBullet() {
        Bullet tempBullet = Instantiate(bullet, firePos.position, transform.rotation).GetComponent<Bullet>();
        tempBullet.speed = shotSpeed;
        SetAttackDir();
        tempBullet.bulletDir = attackDirection;
        tempBullet.popTime = 10f;
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(coolTime);
        canAttack = true;
    }
}
