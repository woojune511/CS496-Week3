using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어의 움직임을 제어하는 스크립트
public class PlayerMoveControl : MovingObject
{
    private static float  animSpeedFactor = (0.8f / 3); // 애니메이션 속도 조절용 상수
    public static Vector2 playerDirection; // 플레이어가 바라보고 있는 방향
    bool isPlayerMoving = false;

    private Animator animator;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    protected override void Start()
    {   
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        // Call the Start function of the MovingObject base class.
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        isPlayerMoving = false;
        SetVelocity();
    }

    // 플레이어의 입력으로 부터 벡터값을 얻는다.
    protected override Vector2 GetVector() {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 입력이 없는 상태가 아니라면 바라보고 있는 방향 업데이트 한다.
        if(inputVector.magnitude > Mathf.Epsilon)
        {
            isPlayerMoving = true;
            playerDirection = inputVector;
        }

        UpdateAnimationParameters(inputVector);


        return inputVector;
    }

    private void UpdateAnimationParameters(Vector2 vector) {
        animator.SetFloat("MoveX", vector.x);
        animator.SetFloat("MoveY", vector.y);
        animator.SetBool("IsPlayerMoving", isPlayerMoving);
        animator.SetFloat("LastMoveY", playerDirection.y);
    }

    private void UpdateAnimationSpeed() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Move")) {
            // 플레이어 속도가 변하면 그에 맞춰서 애니메이션도 빠르게 한다
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other) {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy") {
            playerHealth.PlayerHit();
        }
    }

}
