using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    // 定义方向的枚举
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("房间信息")]
    public GameObject roomPrefab; // 房间的预制件
    private GameObject endRoom;    // 结束房间

    [Header("位置控制")]
    public Transform generatorPoint; // 初始生成房间的位置
    public float xOffset;            // 房间在x轴上的偏移量
    public float yOffset;            // 房间在y轴上的偏移量
    public LayerMask roomLayer;      // 房间层，用于检测重叠
    public int maxStep;              // 从起点到当前房间的最大步数
    public int maxRoomCount;         // 最大房间数量

    public List<Room> rooms = new List<Room>(); // 生成的房间列表

    // 保存远房间、次远房间和单通道房间的列表
    List<GameObject> farRooms = new List<GameObject>();
    List<GameObject> lessFarRooms = new List<GameObject>();
    List<GameObject> oneWayRooms = new List<GameObject>();

    public WallType wallType; // 墙的类型，用于不同门数量的房间

    private Vector3[] directions; // 存储各个方向的向量

    void Start()
    {
        // 初始化方向数组
        directions = new Vector3[]
        {
            new Vector3(0, yOffset, 0),   // 上
            new Vector3(0, -yOffset, 0),  // 下
            new Vector3(-xOffset, 0, 0),  // 左
            new Vector3(xOffset, 0, 0)    // 右
        };

        // 开始协程生成房间
        StartCoroutine(GenerateRoomsCoroutine());
    }

    // 生成房间的协程
    IEnumerator GenerateRoomsCoroutine()
    {
        List<Vector3> currentPositions = new List<Vector3> { generatorPoint.position };  // 初始位置
        List<Vector3> nextPositions = new List<Vector3>();  // 保存下一轮生成房间的位置

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

                    // 如果新位置没有重叠且有效，生成房间
                    if (!Physics2D.OverlapCircle(newPos, 0.2f, roomLayer) && IsValidPosition(newPos))
                    {
                        rooms.Add(Instantiate(roomPrefab, newPos, Quaternion.identity).GetComponent<Room>());
                        nextPositions.Add(newPos);

                        // 检查是否超出大正方形范围
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

            currentPositions = new List<Vector3>(nextPositions); // 更新当前生成的位置列表
            nextPositions.Clear(); // 清空下一轮的位置列表

            yield return null; // 等待下一帧继续

            foreach (var room in rooms)
            {
                SetupRoom(room, room.transform.position); // 设置房间墙体
            }
        }
    }

    // 判断位置是否超出大正方形的边界
    private bool IsOutOfBoundary(Vector3 position)
    {
        float halfSize = (maxRoomCount / 2) * xOffset;  // 大正方形的一半长度
        return Mathf.Abs(position.x) > halfSize || Mathf.Abs(position.y) > halfSize;
    }

    // 检查停止条件
    private bool CheckStopCondition()
    {
        int maxRooms = 24; // 可以根据复杂性或其他条件动态计算
        return rooms.Count >= maxRooms;
    }

    // 检查位置是否有效（没有重叠）
    private bool IsValidPosition(Vector3 position)
    {
        return !Physics2D.OverlapCircle(position, 0.2f, roomLayer);
    }

    // 设置房间的墙壁和门
    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        float roomWidth = xOffset;
        float roomHeight = yOffset;

        // 检查上下左右是否有相邻房间
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, roomHeight, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -roomHeight, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-roomWidth, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(roomWidth, 0, 0), 0.2f, roomLayer);

        newRoom.UpdateRoom(xOffset, yOffset); // 更新房间数据

        // 根据门的数量生成对应的墙
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

    // 寻找结束房间
    public void FindEndRoom()
    {
        // 获取最大步数
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;
        }

        // 收集最大步数和次大步数的房间
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep)
                farRooms.Add(room.gameObject);
            if (room.stepToStart == maxStep - 1)
                lessFarRooms.Add(room.gameObject);
        }

        // 从远房间和次远房间中找出只有一个门的房间
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

        // 随机选择一个作为结束房间
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

// 定义墙体类型的类
[System.Serializable]
public class WallType
{
    // 不同类型的墙体
    public GameObject singleLeft, singleRight, singleUp, singleBottom,
                      doubleUL, doubleLR, doubleBL, doubleUR, doubleUB, doubleBR,
                      tripleULR, tripleUBL, tripleUBR, tripleBLR,
                      fourDoors;
}
