using System;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int[] activeDoors; // [down, right, up, left]
    public int direction; // 0 = down, 1 = right, 2 = up, 3 = left
    public float size = 9.9f;
    public int numberOfDoors;  // Delete if not used by branching algo
    public GameObject prefab;

    private string name;
   
    public Room(string name, GameObject prefab)
    {
        this.name = name;
        this.prefab = prefab;
        this.direction = 0;

        if (name == "deadend")
        {
            this.activeDoors = new int[] { 1, 0, 0, 0 };
            this.numberOfDoors = 1;
        }   
        else if (name == "corridor")
        {
            this.activeDoors = new int[] { 1, 0, 1, 0 };
            this.numberOfDoors = 2;
        }    
        else if (name == "corner")
        {
            this.activeDoors = new int[] { 1, 0, 0, 1 };
            this.numberOfDoors = 2;
        }
        else if (name == "fork")
        {
            this.activeDoors = new int[] { 1, 1, 0, 1 };
            this.numberOfDoors = 3;
        }
        else if (name == "cross")
        {
            this.activeDoors = new int[] { 1, 1, 1, 1 };
            this.numberOfDoors = 4;
        }
    }

    public void Rotate(int rotation)
    {
        int steps = (rotation / 90) % 4;
        if (steps == 0 || this.name == "cross") return;

        int[] newDoors = new int[4];

        for (int i = 0; i < 4; i++)
            newDoors[(i + steps) % 4] = activeDoors[i];

        activeDoors = newDoors;

        direction = (direction + steps) % 4;
    }
}

public class DungeonGenerator : MonoBehaviour
{
    [Header("Room Prefabs")]
    public GameObject deadEndPrefab;
    public GameObject corridorPrefab;
    public GameObject cornerPrefab;
    public GameObject forkPrefab;
    public GameObject crossPrefab;

    [Header("Generation Settings")]
    public int backboneLength = 3;
    public int branchLength = 3;
    [Range(0f, 1f)] public float branchChance = 0.3f;


    private List<Room> allRooms = new List<Room>();
    private HashSet<Vector2Int> occupiedSpaces = new HashSet<Vector2Int>();

    void Start()
    {
        GenerateBackbone();
    }

    void GenerateBackbone()
    {
        Vector2Int gridPosition = Vector2Int.zero;
        
        // First room
        Room firstRoom = new Room("deadend", deadEndPrefab);
        firstRoom.Rotate(new int[] { 0, 90, 180, 270 }[UnityEngine.Random.Range(0, 4)]);

        occupiedSpaces.Add(gridPosition);
        allRooms.Add(firstRoom);
        PlaceRoom(firstRoom, gridPosition);

        int entryDoorDirection = Array.IndexOf(firstRoom.activeDoors, 1);
        Vector2Int moveVector = DirectionToVector(entryDoorDirection);

        for (int i = 0; i < backboneLength - 2; i++)
        {
            // Pick a room type
            Room nextRoom = UnityEngine.Random.value < 0.3f
                ? new Room("corridor", corridorPrefab)
                : new Room("corner", cornerPrefab);

            // Random initial rotation (to encourage variety)
            int rotation = new int[] { 0, 90, 180, 270 }[UnityEngine.Random.Range(0, 4)];
            nextRoom.Rotate(rotation);

            gridPosition += moveVector;
            AlignRoomToPrevious(nextRoom, entryDoorDirection, gridPosition, occupiedSpaces);

            occupiedSpaces.Add(gridPosition);
            allRooms.Add(nextRoom);
            PlaceRoom(nextRoom, gridPosition);

            for (int j = 0; j < 4; j++)
            {
                if (j != ((entryDoorDirection + 2) % 4) && nextRoom.activeDoors[j] == 1)
                {
                    entryDoorDirection = j;
                    moveVector = DirectionToVector(j);
                    break;
                }
            }
        }

        // Last room
        Room lastRoom = new Room("deadend", deadEndPrefab);
        lastRoom.Rotate(new int[] { 0, 90, 180, 270 }[UnityEngine.Random.Range(0, 4)]);

        gridPosition += moveVector;
        AlignRoomToPrevious(lastRoom, entryDoorDirection, gridPosition, occupiedSpaces);

        occupiedSpaces.Add(gridPosition);
        allRooms.Add(lastRoom);
        PlaceRoom(lastRoom, gridPosition);
    }


    // Helper functions
    Vector2Int DirectionToVector(int direction)
    {
        switch (direction)
        {
            case 0: return Vector2Int.down;
            case 1: return Vector2Int.right;
            case 2: return Vector2Int.up;
            case 3: return Vector2Int.left;
            default: return Vector2Int.zero;
        }
    }

    void PlaceRoom(Room room, Vector2Int position)
    {
        Vector3 worldPos = new Vector3(position.x * room.size, position.y * room.size, 0);
        Quaternion rotation = Quaternion.Euler(0, 0, room.direction * 90);
        Instantiate(room.prefab, worldPos, rotation, transform);
    }

    bool IsRotationValid(Room nextRoom, int prevExitDirection, Vector2Int gridPosition, HashSet<Vector2Int> occupiedSpaces)
    {
        int opposite = (prevExitDirection + 2) % 4;

        if (nextRoom.activeDoors[opposite] != 1)
            return false;

        for (int i = 0; i < 4; i++)
        {
            if (i == opposite) continue;

            if (nextRoom.activeDoors[i] == 1)
            {
                Vector2Int exitDir = DirectionToVector(i);
                Vector2Int potentialNextPos = gridPosition + exitDir;

                if (occupiedSpaces.Contains(potentialNextPos))
                    return false;
            }
        }

        return true;
    }

    void AlignRoomToPrevious(Room nextRoom, int prevExitDirection, Vector2Int gridPosition, HashSet<Vector2Int> occupiedSpaces)
    {
        for (int i = 0; i < 4; i++)
        {
            if (IsRotationValid(nextRoom, prevExitDirection, gridPosition, occupiedSpaces))
                return;

            nextRoom.Rotate(90);
        }
    }
}
