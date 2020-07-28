using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MovingObject
{
    public Vector2 bulletDir; // 총알의 방향
    private Animator animator;
    public GameObject explosionEffect;
    
    public float popTime = 100f;
    private float bulletDamage;


    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        if(gameObject.tag == "Bullet")
       { 
            PlayerAttack playerAttack = GameObject.FindWithTag("Player").GetComponent<PlayerAttack>();
            speed = playerAttack.shotSpeed;
            bulletDir = playerAttack.playerAttackDirection;
            bulletDamage = playerAttack.damage;
            popTime = playerAttack.range / speed;
            
        }
        base.Start();
        SetVelocity();
        Invoke("popBullet", popTime ); // 사정 거리만큼 가면 터진다 (나누기는 무거운 operation이므로 바꿀 수 있으면 바꾸자)
    }

    // Update is called once per frame
    void Update()
    {
        SetVelocity();
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    

    // 투사체가 터질 때 호출되는 함수
    protected void popBullet() {
        DestroyBullet();
        Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        if(gameObject.tag == "Bullet")
        {
            if (other.tag == "Bullet" || other.tag == "Player" || other.tag == "EnemyBullet")
                return;
            if (other.tag == "Enemy" || other.tag == "BossEnemy")
            {
                // TODO: 적에게 피해 주기
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                enemyHealth.EnemyHit(bulletDamage);
            }
            popBullet();
        }   
        else if (gameObject.tag == "EnemyBullet") 
        {
            if (other.tag == "Bullet" || other.tag == "Enemy" || other.tag == "EnemyBullet" || other.tag == "BossEnemy")
                return;
            if (other.tag == "Player")
            {
                // TODO: 플레이어에게 피해 주기
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                playerHealth.PlayerHit(other);
            }
            popBullet();
        } 
    }

    protected override Vector2 GetVector() {
        // 플레이어의 시선 방향으로 총알이 나가도록 설정
        return bulletDir;
    }

}
