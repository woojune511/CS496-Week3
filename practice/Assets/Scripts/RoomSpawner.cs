using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using rnd = System.Random;
using System;


public class RoomSpawner: MonoBehaviour{
    
    List<Vector3> spawnedPositions = new List<Vector3>();
    RoomTreeNode topNode;
    List<RoomTreeNode> leafList = new List<RoomTreeNode>();
    RoomTreeNode bossRoom;
    
    int max_iter = 15;
    int min_room = 7;
    [HideInInspector] int iter = 0;
    [HideInInspector] float furthest = 0f;

    rnd rand = new rnd(Guid.NewGuid().GetHashCode());

    void Start(){
        // Invoke("Spawn", 0.1f);
        while(spawnedPositions.Count < min_room)
            SpawnRoom();
        bossRoom.room.isBossRoom = true;
        bossRoom.room.RenderRoom();
    }

    void ResetParams(){
        iter = 0;
        furthest = 0;
        topNode = null;
        if(bossRoom != null)
            bossRoom.room.isBossRoom = false;
        bossRoom = null;
        leafList.Clear();
        spawnedPositions.Clear();
    }

    void SpawnRoom(){
        ResetParams();
        Room initialRoom = new Room(new bool[]{true, true, true, true}, new Vector3(0,0,0));
        spawnedPositions.Add(new Vector3(0,0,0));
        initialRoom.RenderRoom();
        topNode = new RoomTreeNode(initialRoom);
        
        CreateRoomTraverse(topNode);

        SpawnRoomTrasverse(topNode);

        Debug.Log($"{bossRoom.room.room_position.x} {bossRoom.room.room_position.y} {bossRoom.room.room_position.z}");

        // foreach(Vector3 a in spawnedPositions){
        //     Debug.Log($"{a.x} {a.y} {a.z}");    
        // }
    }

    void CreateRoomTraverse(RoomTreeNode rmnd){
        if(iter >= max_iter) return;
        for(int i=0; i<4; i++){
            if(rmnd.room.room_orient[i]){
                bool[] temp = rndBoolArr();
                switch(i){
                    case 0:
                        temp[1] = true;
                        var newCoord1 = rmnd.room.room_position + new Vector3(0, 10, 0);
                        // if(Array.Exists(spawnedPositions, element => element == newCoord1)) continue;
                        if(spawnedPositions.Contains(newCoord1)) continue;
                        spawnedPositions.Add(newCoord1);
                        var newRoom1 = new Room(temp, newCoord1);
                        rmnd.AddChild(new RoomTreeNode(newRoom1));
                        break;
                    case 1:
                        temp[0] = true;
                        var newCoord2 = rmnd.room.room_position + new Vector3(0, -10, 0);
                        // if(Array.Exists(spawnedPositions, element => element == newCoord2)) continue;
                        if(spawnedPositions.Contains(newCoord2)) continue;
                        spawnedPositions.Add(newCoord2);
                        var newRoom2 = new Room(temp, newCoord2);
                        rmnd.AddChild(new RoomTreeNode(newRoom2));
                        break;
                    case 2:
                        temp[3] = true;
                        var newCoord3 = rmnd.room.room_position + new Vector3(-16, 0, 0);
                        // if(Array.Exists(spawnedPositions, element => element == newCoord3)) continue;
                        if(spawnedPositions.Contains(newCoord3)) continue;
                        spawnedPositions.Add(newCoord3);
                        var newRoom3 = new Room(temp, newCoord3);
                        rmnd.AddChild(new RoomTreeNode(newRoom3));
                        break;
                    case 3:
                        temp[2] = true;
                        var newCoord4 = rmnd.room.room_position + new Vector3(16, 0, 0);
                        // if(Array.Exists(spawnedPositions, element => element == newCoord4)) continue;
                        if(spawnedPositions.Contains(newCoord4)) continue;
                        spawnedPositions.Add(newCoord4);
                        var newRoom4 = new Room(temp, newCoord4);
                        rmnd.AddChild(new RoomTreeNode(newRoom4));
                        break;
                }
                iter++;
            }
        }

        foreach(RoomTreeNode child in rmnd.ChildNodes){
            CreateRoomTraverse(child);
        }

    }

    void SpawnRoomTrasverse(RoomTreeNode rmnd){
        if(rmnd.IsLeaf){
            leafList.Add(rmnd);
            if(!spawnedPositions.Contains(rmnd.room.room_position + new Vector3(0, 10, 0)))
                rmnd.room.room_orient[0] = false;
            if(!spawnedPositions.Contains(rmnd.room.room_position + new Vector3(0, -10, 0)))
                rmnd.room.room_orient[1] = false;
            if(!spawnedPositions.Contains(rmnd.room.room_position + new Vector3(-16, 0, 0)))
                rmnd.room.room_orient[2] = false;
            if(!spawnedPositions.Contains(rmnd.room.room_position + new Vector3(16, 0, 0)))
                rmnd.room.room_orient[3] = false;
        }
        else{
            foreach(RoomTreeNode child in rmnd.ChildNodes){
                SpawnRoomTrasverse(child);
            }
        }

        if(spawnedPositions.Contains(rmnd.room.room_position + new Vector3(0, 10, 0)))
            rmnd.room.room_orient[0] = true;
        if(spawnedPositions.Contains(rmnd.room.room_position + new Vector3(0, -10, 0)))
            rmnd.room.room_orient[1] = true;
        if(spawnedPositions.Contains(rmnd.room.room_position + new Vector3(-16, 0, 0)))
            rmnd.room.room_orient[2] = true;
        if(spawnedPositions.Contains(rmnd.room.room_position + new Vector3(16, 0, 0)))
            rmnd.room.room_orient[3] = true;

        if(Math.Abs(rmnd.room.room_position.x) > Math.Abs(furthest)){
            furthest = rmnd.room.room_position.x;
            bossRoom = rmnd;
            Debug.Log($"furthest coordinate {rmnd.room.room_position.x}");
        }
        if(Math.Abs(rmnd.room.room_position.y) > Math.Abs(furthest)){
            furthest = rmnd.room.room_position.y;
            bossRoom = rmnd;
            Debug.Log($"furthest coordinate {rmnd.room.room_position.y}");
        }

        rmnd.room.RenderRoom();
    }

    bool rndBool(){
        return (rand.Next(3) == 0);
    }

    bool[] rndBoolArr(){
        return new bool[]{rndBool(), rndBool(), rndBool(), rndBool()};
    }

}