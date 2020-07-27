using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 움직일 수 있는 오브젝트(플레이어, 적, 투사체 등)의 운동을 정의하는 스크립트
public abstract class MovingObject : MonoBehaviour
{
    public float speed;
    // public float moveTime = 0.1f;
    // public LayerMask blockingLayer; // Layer on which collision will be checked.

    private Collider2D Collider;
    private Rigidbody2D rigidBody;
    // private float inverseMoveTime;

    private Vector2 vector;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Collider = GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        // inverseMoveTime = 1f / moveTime; // reciprocal => multiplying is more efficient than dividing
    }

    // MovingObject를 상속받은 오브젝트의 velocity를 바꾼다.
    protected void SetVelocity()
    {
        vector = GetVector();

        rigidBody.velocity = vector.normalized * speed;
    }

    // 적절한 방식으로 벡터값을 얻는다.
    protected abstract Vector2 GetVector();

    protected abstract void OnTriggerEnter2D(Collider2D other);

}
