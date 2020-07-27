using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerCam : MonoBehaviour{
    // public GameObject cam = GameObject.Find("Main Camera");
    public GameObject player;
    public Transform tr;
    public Camera cam;

    public Vector3 playerPosition;

    void Start(){
        Debug.Log("Start called");
        player = GameObject.Find("Player");
        tr = GetComponent<Transform>();
        // playerPosition = player.GetComponent<Transform>().position;
        tr.position = new Vector3(7.5f, 4.5f, -10);
        // cam = GetComponent<Camera>();
        // cam.transform.SetPositionAndRotation(new Vector3(7.5f, 4.5f, -10), Quaternion.identity);
        
        // cam.GetComponent<MouseLook>().enabled = false;
    }

    void LateUpdate(){
        playerPosition = player.GetComponent<Transform>().position;
        tr.position = new Vector3((float)(Math.Floor(playerPosition.x/16)*16+7.5), (float)(Math.Floor(playerPosition.y/10)*10+4.5), -10);
        // cam.transform.SetPositionAndRotation(new Vector3((float)((int)(playerPosition.x/16)*16+7.5), (float)((int)(playerPosition.y/10)*10+4.5), -10), Quaternion.identity);
    }
}