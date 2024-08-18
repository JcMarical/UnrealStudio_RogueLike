using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("房间信息")]
    public GameObject roomPrefab;
    private GameObject endRoom;

    [Header("位置控制")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;
    public LayerMask roomLayer;
    public int maxStep;
    public int maxRoomCount;

    public List<Room> rooms = new List<Room>();

    List<GameObject> farRooms = new List<GameObject>();

    List<GameObject> lessFarRooms = new List<GameObject>();

    List<GameObject> oneWayRooms = new List<GameObject>();

    public WallType wallType;

    private Vector3[] directions;

    void Start()
    {
        directions = new Vector3[]
       {
            new Vector3(0, yOffset, 0),   // up
            new Vector3(0, -yOffset, 0),  // down
            new Vector3(-xOffset, 0, 0),  // left
            new Vector3(xOffset, 0, 0)    // right
       };

        StartCoroutine(GenerateRoomsCoroutine());
    }

    IEnumerator GenerateRoomsCoroutine()
    {
        List<Vector3> currentPositions = new List<Vector3> { generatorPoint.position };  // 初始位置
        List<Vector3> nextPositions = new List<Vector3>();

        bool stopGeneration = false;
        while (!stopGeneration && rooms.Count < maxRoomCount)
        {
            for (int i = 0; i < currentPositions.Count; i++)
            {
                Vector3 currentPos = currentPositions[i];

                // 随机选择一个方向
                List<int> availableDirections = new List<int> { 0, 1, 2, 3 };
                while (availableDirections.Count > 0)
                {
                    int randomIndex = Random.Range(0, availableDirections.Count);
                    int directionIndex = availableDirections[randomIndex];
                    availableDirections.RemoveAt(randomIndex);

                    Vector3 newPos = currentPos + directions[directionIndex];

                    if (!Physics2D.OverlapCircle(newPos, 0.2f, roomLayer) && IsValidPosition(newPos))
                    {
                        rooms.Add(Instantiate(roomPrefab, newPos, Quaternion.identity).GetComponent<Room>());
                        nextPositions.Add(newPos);

                        // 检查是否超出大正方形
                        if (IsOutOfBoundary(newPos))
                        {
                            stopGeneration = true;
                            break;
                        }

                        // 每次只在一个方向生成一个房间
                        break;
                    }
                }

                if (stopGeneration)
                    break;
            }

            currentPositions = new List<Vector3>(nextPositions);
            nextPositions.Clear();

            yield return null;

            foreach (var room in rooms)
            {
                SetupRoom(room, room.transform.position);
            }
        }
    }
    private bool IsOutOfBoundary(Vector3 position)
    {
        float halfSize = (maxRoomCount / 2) * xOffset;  // 大正方形的一半长度
        return Mathf.Abs(position.x) > halfSize || Mathf.Abs(position.y) > halfSize;
    }

    private bool CheckStopCondition()
    {
        int maxRooms = 24; // 可以根据复杂性或其他条件动态计算
        return rooms.Count >= maxRooms;
    }

    private bool IsValidPosition(Vector3 position)
    {
        return !Physics2D.OverlapCircle(position, 0.2f, roomLayer);
    }

    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        float roomWidth = xOffset;
        float roomHeight = yOffset;

        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, roomHeight, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -roomHeight, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-roomWidth, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(roomWidth, 0, 0), 0.2f, roomLayer);

        newRoom.UpdateRoom(xOffset, yOffset);

        switch (newRoom.doorNumber)
        {
            case 1:
                if (newRoom.roomUp)
                    Instantiate(wallType.singleUp, roomPosition, Quaternion.identity);
                if (newRoom.roomDown)
                    Instantiate(wallType.singleBottom, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft)
                    Instantiate(wallType.singleLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomRight)
                    Instantiate(wallType.singleRight, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomUp && newRoom.roomLeft)
                    Instantiate(wallType.doubleUL, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.doubleLR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.doubleBL, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.doubleUR, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.doubleBR, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.doubleUB, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (!newRoom.roomUp)
                    Instantiate(wallType.tripleBLR, roomPosition, Quaternion.identity);
                if (!newRoom.roomDown)
                    Instantiate(wallType.tripleULR, roomPosition, Quaternion.identity);
                if (!newRoom.roomLeft)
                    Instantiate(wallType.tripleUBR, roomPosition, Quaternion.identity);
                if (!newRoom.roomRight)
                    Instantiate(wallType.tripleUBL, roomPosition, Quaternion.identity);
                break;
            case 4:
                if (newRoom.roomLeft && newRoom.roomRight && newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.fourDoors, roomPosition, Quaternion.identity);
                break;
        }
    }

    public void FindEndRoom()
    {
        //最大数值
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;
        }

        //获得最大值房间和次大值
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep)
                farRooms.Add(room.gameObject);
            if (room.stepToStart == maxStep - 1)
                lessFarRooms.Add(room.gameObject);
        }

        for (int i = 0; i < farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNumber == 1)
                oneWayRooms.Add(farRooms[i]);
        }

        for (int i = 0; i < lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNumber == 1)
                oneWayRooms.Add(lessFarRooms[i]);
        }

        if (oneWayRooms.Count != 0)
        {
            endRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];
        }
        else
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];
        }
    }
}




[System.Serializable]
public class WallType
{
    public GameObject singleLeft, singleRight, singleUp, singleBottom,
                      doubleUL, doubleLR, doubleBL, doubleUR, doubleUB, doubleBR,
                      tripleULR, tripleUBL, tripleUBR, tripleBLR,
                      fourDoors;
}
