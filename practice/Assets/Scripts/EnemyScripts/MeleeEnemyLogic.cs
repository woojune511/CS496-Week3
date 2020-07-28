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
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        SetVelocity();
    }

    protected override Vector2 GetVector() {
        Vector2 playerPos = playerTransform.position;
        Vector2 enemyPos = enemyTransfrom.position;

        return playerPos - enemyPos ;
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        // 딱히 할 거 없는듯
    }

}
