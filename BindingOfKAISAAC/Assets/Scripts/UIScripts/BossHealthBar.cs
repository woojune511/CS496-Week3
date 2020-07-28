using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider bossHealthBar;
    [SerializeField]
    private EnemyHealth bossHealth;
    private float bossMaxHealthInverse;

    private bool wasSpawn = false;

    void Start() {
        wasSpawn = false;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (!GameManager.instance.isBossRoomEntered)
            return;

        if(wasSpawn && GameManager.instance.isBossDead) {
            bossHealthBar.gameObject.SetActive(false);
            return;
        }
        
        if(!GameManager.instance.isBossDead && !wasSpawn) {
            wasSpawn = true;
            bossHealthBar.gameObject.SetActive(true);
            Invoke("SetBossHealth", 1.0f);
        }

        if(bossHealth != null)
            bossHealthBar.value = bossHealth.health * bossMaxHealthInverse * 100;
        
    }



    private void SetBossHealth() {
        bossHealth = GameObject.FindGameObjectWithTag("BossEnemy").GetComponent<EnemyHealth>();
        bossMaxHealthInverse = 1 / bossHealth.health;
    }
}
