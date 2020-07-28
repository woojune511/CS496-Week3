using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    // -------------------------------[PLAYER STATUS]-------------------------------
    public int health;
    public int maxHealth;
    // -----------------------------------------------------------------------------
    public bool canBeHit = true;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Animator animator;
    public SpriteRenderer spriteRenderer;


    void Start() {
        canBeHit = true;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHearts();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    // 현재의 체력에 맞추어 하트 UI를 업데이트 하는 함수
    void UpdateHearts() {
        if (health > maxHealth) 
        {
            health = maxHealth;
        }

        int healthIndex = health >> 1;

        for (int i = 0; i < hearts.Length; i++) {
            if(i < healthIndex) {
                hearts[i].sprite = fullHeart;
            } 
            else if(i == healthIndex && (health % 2 == 1)) 
            {
                hearts[i].sprite = halfHeart;
            }
            else {
                hearts[i].sprite = emptyHeart;
            }

            if(i < (maxHealth >> 1)) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }


    // 플레이어가 피격당했을 때 => 1. 체력이 깎인다(+ 하트 업데이트) 2. 게임오버인지 체크한다 3. 피격 모션이 재생된다 4. 피격 모션이 재생되는 동안은 무적
    public void PlayerHit() {
        if(canBeHit)
        {
            health--;
            if(isGameOver()) {
                GameOver();
                return;
            }
            UpdateHearts();
            // 무적 시간 시작
            canBeHit = false;
            // 피격 애니메이션 재생
            StartCoroutine(UnBeatTime());
        }
    }

    public void GameOver() {
        // TODO : 죽는 모션 재생하기
        Destroy(gameObject);
        // TODO: 게임오버 화면으로 넘어가기
    }

    IEnumerator UnBeatTime() {
        // 피격 애니메이션 재생 => 일정 시간 후 피격 애니메이션 끝나고 무적 시간 끝

        int countTime = 0;

        while(countTime < 10) {
            // 알파값 늘였다 줄였다 하기 => 깜빡거림
            if(countTime == 0) 
            {
                // 시작은 빨갛게
                spriteRenderer.color = new Color32(255, 0, 0, 180);
            }
            else if (countTime % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else 
                spriteRenderer.color = new Color32(255, 255, 255, 180);

            // Wait Update Frame
            yield return new WaitForSeconds(0.2f);

            countTime++;
        }

        // 피격 이펙트 끝내기
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        // 무적 시간 끝
        canBeHit = true;

        yield return null;
    }

    bool isGameOver() {
        return health <= 0;
    }

}
