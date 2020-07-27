using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.Contracts;


public class Room: MonoBehaviour
{
    // public bool[] room_orient = new bool[4]; // Indicates if there is a door on the top, bottom, left, right side
    [HideInInspector] public bool[] room_orient;
    [HideInInspector] public int roomSize_x = 16;
    [HideInInspector] public int roomSize_y = 10;
    [HideInInspector] public Vector3 room_position;
    [HideInInspector] public bool isBossRoom = false;

    public Room(bool[] orient, Vector3 pos){
        Contract.Requires(orient.Length == 4);
        room_orient = orient;
        room_position = pos;
    }

    public void RenderRoom(){
        if(isBossRoom){
            GameObject BossIcon = GameObject.Find("Boss Icon");
            Instantiate(BossIcon, room_position + new Vector3(7.5f, 4.5f, -1), Quaternion.identity);
        }
        GetWall();
        GetFloor();
    }

    public void GetWall(){ 
        GameObject WallSprite = GameObject.Find("Wall"); // 나중에 리스트로 바꿔서 랜덤하게 sprite 넣기
        // WallSprite = AssetDatabase.FindAssets("Wall");
        // WallSprite.GetComponent<Renderer>().enabled = false;

        Contract.Requires(room_orient.Length == 4, $"Length of room_orient: {room_orient.Length}");
        for(int i=0; i<roomSize_x; i++){
            if(!room_orient[0] || (i!= 7 && i!= 8))
                Instantiate(WallSprite, room_position + new Vector3(i, 9, 0), Quaternion.identity);
            if(!room_orient[1] || (i!= 7 && i!= 8))
                Instantiate(WallSprite, room_position + new Vector3(i, 0, 0), Quaternion.identity);
        }
        for(int i=0; i<roomSize_y; i++){
            if(!room_orient[2] || (i!= 4 && i != 5))
                Instantiate(WallSprite, room_position + new Vector3(0, i, 0), Quaternion.identity);
            if(!room_orient[3] || (i!= 4 && i != 5))   
                Instantiate(WallSprite, room_position + new Vector3(15, i, 0), Quaternion.identity);
        }

    }

    public void GetFloor(){
        GameObject FloorSprite = GameObject.Find("Floor"); // 나중에 리스트로 바꿔서 랜덤하게 sprite 넣기
        // WallSprite = AssetDatabase.FindAssets("Wall");
        // WallSprite.GetComponent<Renderer>().enabled = false;
        for(int i=0; i<roomSize_x; i++){
            for(int j=0; j<roomSize_y; j++){
                Instantiate(FloorSprite, room_position + new Vector3(i, j, 0), Quaternion.identity);
            }
        }
    }

    void onTriggerEnter2D(Collider2D other){
        ;
    }

    void Start(){
    
    }

}
