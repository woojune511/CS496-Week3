using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using rnd = System.Random;
using System;


public class RoomSpawner: MonoBehaviour{
    

    public GameObject meleeEnemy;
    public GameObject rangedEnemy;
    public GameObject bossEnemy;
    public GameObject item;
    public GameObject halfHeart;
    

    List<RoomTreeNode> Rooms = new List<RoomTreeNode>();
    List<Vector3> spawnedPositions = new List<Vector3>();
    RoomTreeNode topNode;
    List<RoomTreeNode> leafList = new List<RoomTreeNode>();
    RoomTreeNode bossRoom;
    List<RoomTreeNode> itemRooms = new List<RoomTreeNode>();
    int max_iter = 12;
    int min_room = 10;
    [HideInInspector] int iter = 0;
    [HideInInspector] float furthest = 0f;
    public Transform playerTr;
    public RoomTreeNode playRoom;

    private bool isNotSpawning = true;

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
        foreach(RoomTreeNode itrm in itemRooms){
            itrm.room.isItemRoom = false;
        }
        bossRoom = null;
        leafList.Clear();
        spawnedPositions.Clear();
        Rooms.Clear();
        itemRooms.Clear();
    }

    void SpawnRoom(){
        ResetParams();
        Room initialRoom = new Room(new bool[]{true, true, true, true}, new Vector3(0,0,0));
        initialRoom.isCleared = true;
        initialRoom.isSpawned = true;
        initialRoom.itemDropped = true;
        spawnedPositions.Add(new Vector3(0,0,0));
        initialRoom.RenderRoom();
        topNode = new RoomTreeNode(initialRoom);
        playRoom = topNode;
        
        CreateRoomTraverse(topNode);

        SpawnRoomTrasverse(topNode);

        getItemRoom();

        foreach(RoomTreeNode rm in Rooms){
            rm.room.RenderRoom();
        }

        // Debug.Log($"{bossRoom.room.room_position.x} {bossRoom.room.room_position.y} {bossRoom.room.room_position.z}");

        // foreach(Vector3 a in spawnedPositions){
        //     Debug.Log($"{a.x} {a.y} {a.z}");    
        // }
    }

    void CreateRoomTraverse(RoomTreeNode rmnd){
        Rooms.Add(rmnd);
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
            // Debug.Log($"furthest coordinate {rmnd.room.room_position.x}");
        }
        if(Math.Abs(rmnd.room.room_position.y) > Math.Abs(furthest)){
            furthest = rmnd.room.room_position.y;
            bossRoom = rmnd;
            // Debug.Log($"furthest coordinate {rmnd.room.room_position.y}");
        }

        // rmnd.room.RenderRoom();
    }

    // topNode, BossRoom을 제외한 나머지 방들 중에서 2개를 item room으로
    void getItemRoom(){
        Contract.Requires(itemRooms.Count <= 2);
        if(itemRooms.Count == 2) return;
        int rndIndex = rand.Next(Rooms.Count);
        RoomTreeNode rndRoom = Rooms[rndIndex];
        if(rndRoom != topNode && rndRoom != bossRoom && !itemRooms.Contains(rndRoom)){
            itemRooms.Add(rndRoom);
            rndRoom.room.isItemRoom = true;
            // Debug.Log($"item room position {rndRoom.room.room_position.x} {rndRoom.room.room_position.y}");
        }
        getItemRoom();
    }

    bool rndBool(){
        return (rand.Next(3) == 0);
    }

    bool[] rndBoolArr(){
        return new bool[]{rndBool(), rndBool(), rndBool(), rndBool()};
    }

    void DestroyDoors(Vector3 roomPos) {
        List<Vector3> adjacentWalls = new List<Vector3>(){roomPos + new Vector3(0,4,0), roomPos + new Vector3(0,5,0), roomPos + new Vector3(7,0,0),
        roomPos + new Vector3(8,0,0), roomPos + new Vector3(-1,4,0), roomPos + new Vector3(-1,5,0), roomPos + new Vector3(7,-1,0), roomPos + new Vector3(8,-1,0),
        roomPos + new Vector3(7,9,0), roomPos + new Vector3(7,10,0), roomPos + new Vector3(8,9,0), roomPos + new Vector3(8,10,0), roomPos + new Vector3(15,4,0),
        roomPos + new Vector3(16,4,0), roomPos + new Vector3(15,5,0), roomPos + new Vector3(16,5,0)};

        foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door")) {
            if(adjacentWalls.Contains(door.transform.position))
                Destroy(door, 0.5f);
        }
    }

    int getMonsterCount(){
        if(playRoom.room.isBossRoom) {
            if (GameManager.instance.isBossDead)
                return 0;
            return 1;
        }
        
        int count = 0;
        foreach(GameObject ememy in GameObject.FindGameObjectsWithTag("Enemy")){
            Vector3 enemyPos = ememy.transform.position;
            Vector3 roomPos = playRoom.room.room_position;
            if(enemyPos.x > roomPos.x && enemyPos.x < roomPos.x + 16 && enemyPos.y > roomPos.y && enemyPos.y < roomPos.y + 10)
                count++;
        }
        return count;
    }

    void Update(){
        Vector3 playerPos = playerTr.position;
        // Debug.Log($"{playRoom.room.room_position} {playerPos} {getRoomPosition(playerPos)} {getMonsterCount()}");
        if(playRoom.room.room_position != getRoomPosition(playerPos)) {
            // if(room_position == getRoomPosition(playerPos)){
                // Debug.Log($"ROOM CHANGED, ROOM POSITION: {playRoom.room.room_position.x} {playRoom.room.room_position.y}");
                playRoom = findRoombyPosition(getRoomPosition(playerPos));
            // }    
        }
        if(playRoom.room.isBossRoom) {
            GameManager.instance.isBossRoomEntered = true;
        }
        if(!playRoom.room.isCleared) {
            if(!playRoom.room.isSpawned)
                if (isNotSpawning) {
                    isNotSpawning = false;
                    StartCoroutine(DelayedEnemySpawn());
            }
            Invoke("CheckRoomCleared", 1.5f);
        } else {
            DestroyDoors(playRoom.room.room_position);
            if (!playRoom.room.itemDropped) {
                DropHeartRandomly();
                DropItem();
            }
            
        }

    }

    void CheckRoomCleared() {
        if(getMonsterCount() == 0){
            playRoom.room.isCleared = true;
        }
    }

    void DropItem() {
        Instantiate(item, playRoom.room.room_position + new Vector3(8, 5, 0), Quaternion.identity);
        playRoom.room.itemDropped = true;
    }

    void DropHeartRandomly() {
        if(rand.Next(5) <= 3) return;

        Instantiate(halfHeart, playRoom.room.room_position + new Vector3(6, 4, 0), Quaternion.identity);
    }

    IEnumerator DelayedEnemySpawn() {
        yield return new WaitForSeconds(0.5f);
        SpawnEnemy();
        yield return new WaitForSeconds(0.5f);      
        if(getMonsterCount() != 0)
            playRoom.room.GetDoor();
        isNotSpawning = true;
        yield return null;
    }

    void SpawnEnemy() {
        if(playRoom.room.isBossRoom) {
            Instantiate(bossEnemy, playRoom.room.room_position + new Vector3(8, 5, 0), Quaternion.identity);
            GameManager.instance.isBossDead = false;
            playRoom.room.isSpawned = true;
            return;
        }
        
        
        int enemy_num = rand.Next(3,7);
        for(int i=0; i<enemy_num; i++){
            bool isMelee = rndBool();

            float enemyX = (float)(playRoom.room.room_position.x + 8 + (rand.NextDouble() - 0.5) * 6);
            float enemyY = (float)(playRoom.room.room_position.y + 5 + (rand.NextDouble() - 0.5) * 4);
            if(isMelee){
                Instantiate(meleeEnemy, new Vector2(enemyX, enemyY), Quaternion.identity);
            }
            else{
                //spawn ranged
                Instantiate(rangedEnemy, new Vector2(enemyX, enemyY), Quaternion.identity);
            }
        }
        
        playRoom.room.isSpawned = true;
        
    }

    Vector3 getRoomPosition(Vector3 pos){
        return new Vector3((float)Math.Floor(pos.x/16)*16, (float)Math.Floor(pos.y/10)*10, 0);
    }

    RoomTreeNode findRoombyPosition(Vector3 pos){
        Vector3 roomPos = getRoomPosition(pos);
        foreach(RoomTreeNode rmnd in Rooms){
            if (rmnd.room.room_position == roomPos)
                return rmnd;
        }
        return playRoom;
    }

    void wait(){
        ;
    }

}