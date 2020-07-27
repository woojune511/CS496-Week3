﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform pos;
    public bool canAttack = true;


    // -------------------------------[PLAYER STATUS]-------------------------------
    public float range; // 사정거리
    [SerializeField]
    private Text rangeText;

    public float coolTime; // 공격 딜레이
    [SerializeField]
    private Text attackSpeedText;
    public float damage; // 데미지
    [SerializeField]
    private Text dmgText;
    public float shotSpeed; // 투사체 속도
    [SerializeField]
    private Text shotSpeedText;
    // -----------------------------------------------------------------------------
    public Vector2 playerAttackDirection; // 플레이어가 바라보고 있는 방향

    // Start is called before the first frame update
    void Start()
    {
        UpdateATKStatusText();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerAttackDirection();
        if (IsInputExists() && canAttack) 
        {
            canAttack = false;
            Invoke("ShootBullet", 0.05f);
        }
    }
    
    bool IsInputExists() {
        // return playerAttackDirection.magnitude > Mathf.Epsilon;
        
        return (Input.GetKey(KeyCode.LeftArrow) || 
        (Input.GetKey(KeyCode.RightArrow)) || 
        (Input.GetKey(KeyCode.UpArrow)) ||
        (Input.GetKey(KeyCode.DownArrow)) );
    }

    void GetPlayerAttackDirection() {
        Vector2 inputVector = new Vector2(Input.GetAxisRaw("HorizontalFire"), Input.GetAxisRaw("VerticalFire"));

        if(inputVector.magnitude > Mathf.Epsilon) 
            playerAttackDirection = inputVector;
    }

    void ShootBullet() {
        MakeBullet();
        StartCoroutine(WaitForIt());
    }

    void MakeBullet() {
        Instantiate(bullet, pos.position, transform.rotation);
    }

     IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(coolTime);
        canAttack = true;
    }

    public void UpdateATKStatusText() {
        rangeText.text = $"RAN: {range}";
        attackSpeedText.text = $"ATS: {coolTime}";
        dmgText.text = $"RAN: {damage}";
        shotSpeedText.text = $"RAN: {shotSpeed}";
    }
    

}
