using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyLogic : MovingObject
{
    Transform enemyTransfrom;
    Transform playerTransform;

    // Start is called before the first frame update
    protected override void Start()
    {
        enemyTransfrom = GetComponent<Transform>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        SetVelocity();
    }

    protected override Vector2 GetVector() {
        // 나중에 스포너 형식으로 바꾸면 지워도 될 듯
        if(!GameManager.instance.isStarted)
            return Vector2.zero;
        if (playerTransform == null) {
            playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        Vector2 playerPos = playerTransform.position;
        Vector2 enemyPos = enemyTransfrom.position;

        return playerPos - enemyPos ;
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        // 딱히 할 거 없는듯
        if (other.tag == "Bullet") {
            base.KnockBack(other.attachedRigidbody.velocity);
        }
    }

}
