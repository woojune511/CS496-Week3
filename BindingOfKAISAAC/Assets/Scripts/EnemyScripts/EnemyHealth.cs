using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    SpriteRenderer spriteRenderer;
    public GameObject explosionEffect;
    public Transform popTransfrom;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void EnemyHit(float damage) {
        health -= damage;
        StartCoroutine(PlayHitAnimation());
        if(isDead()){
            Die();
        }
    }

    IEnumerator PlayHitAnimation() {
        spriteRenderer.color = new Color32(255, 0, 0, 180);
        yield return new WaitForSeconds(0.2f);

        // 피격 이펙트 끝내기
        spriteRenderer.color = new Color32(255, 255, 255, 255);

        yield return null;
    }

    bool isDead() {
        return health <= 0;
    }

    void Die() {
        Destroy(gameObject);
        Instantiate(explosionEffect, popTransfrom.position, transform.rotation);
    }

}
