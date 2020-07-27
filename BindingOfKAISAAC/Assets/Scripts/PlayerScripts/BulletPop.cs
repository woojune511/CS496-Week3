using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPop : MonoBehaviour
{
    Animator animator;
    float exitTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(CheckAnimationState());
    }

    // Update is called once per frame
    void Update()
    {
        //if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > exitTime)
        // Destroy(this.gameObject, 0.8f);
    }

    IEnumerator CheckAnimationState()
    {

        while (!animator.GetCurrentAnimatorStateInfo(0)
        .IsName("BulletPop")) 
        { 
            //전환 중일 때 실행되는 부분
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0)
        .normalizedTime < exitTime)
        {
            //애니메이션 재생 중 실행되는 부분
            yield return null;
        }
        
        //애니메이션 완료 후 실행되는 부분
        Destroy(gameObject);
    }
}
