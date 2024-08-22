using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    // ���巽���ö��
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("������Ϣ")]
    public GameObject roomPrefab; // �����Ԥ�Ƽ�
    private GameObject endRoom;    // ��������

    [Header("λ�ÿ���")]
    public Transform generatorPoint; // ��ʼ���ɷ����λ��
    public float xOffset;            // ������x���ϵ�ƫ����
    public float yOffset;            // ������y���ϵ�ƫ����
    public LayerMask roomLayer;      // ����㣬���ڼ���ص�
    public int maxStep;              // ����㵽��ǰ����������
    public int maxRoomCount;         // ��󷿼�����

    public List<Room> rooms = new List<Room>(); // ���ɵķ����б�

    // ����Զ���䡢��Զ����͵�ͨ��������б�
    List<GameObject> farRooms = new List<GameObject>();
    List<GameObject> lessFarRooms = new List<GameObject>();
    List<GameObject> oneWayRooms = new List<GameObject>();

    public WallType wallType; // ǽ�����ͣ����ڲ�ͬ�������ķ���

    private Vector3[] directions; // �洢�������������

    void Start()
    {
        // ��ʼ����������
        directions = new Vector3[]
        {
            new Vector3(0, yOffset, 0),   // ��
            new Vector3(0, -yOffset, 0),  // ��
            new Vector3(-xOffset, 0, 0),  // ��
            new Vector3(xOffset, 0, 0)    // ��
        };

        // ��ʼЭ�����ɷ���
        StartCoroutine(GenerateRoomsCoroutine());
    }

    // ���ɷ����Э��
    IEnumerator GenerateRoomsCoroutine()
    {
        List<Vector3> currentPositions = new List<Vector3> { generatorPoint.position };  // ��ʼλ��
        List<Vector3> nextPositions = new List<Vector3>();  // ������һ�����ɷ����λ��

        bool stopGeneration = false;
        while (!stopGeneration && rooms.Count < maxRoomCount)
        {
            for (int i = 0; i < currentPositions.Count; i++)
            {
                Vector3 currentPos = currentPositions[i];

                // ���ѡ��һ������
                List<int> availableDirections = new List<int> { 0, 1, 2, 3 };
                while (availableDirections.Count > 0)
                {
                    int randomIndex = Random.Range(0, availableDirections.Count);
                    int directionIndex = availableDirections[randomIndex];
                    availableDirections.RemoveAt(randomIndex);

                    Vector3 newPos = currentPos + directions[directionIndex];

                    // �����λ��û���ص�����Ч�����ɷ���
                    if (!Physics2D.OverlapCircle(newPos, 0.2f, roomLayer) && IsValidPosition(newPos))
                    {
                        rooms.Add(Instantiate(roomPrefab, newPos, Quaternion.identity).GetComponent<Room>());
                        nextPositions.Add(newPos);

                        // ����Ƿ񳬳��������η�Χ
                        if (IsOutOfBoundary(newPos))
                        {
                            stopGeneration = true;
                            break;
                        }

                        // ÿ��ֻ��һ����������һ������
                        break;
                    }
                }

                if (stopGeneration)
                    break;
            }

            currentPositions = new List<Vector3>(nextPositions); // ���µ�ǰ���ɵ�λ���б�
            nextPositions.Clear(); // �����һ�ֵ�λ���б�

            yield return null; // �ȴ���һ֡����

            foreach (var room in rooms)
            {
                SetupRoom(room, room.transform.position); // ���÷���ǽ��
            }
        }
    }

    // �ж�λ���Ƿ񳬳��������εı߽�
    private bool IsOutOfBoundary(Vector3 position)
    {
        float halfSize = (maxRoomCount / 2) * xOffset;  // �������ε�һ�볤��
        return Mathf.Abs(position.x) > halfSize || Mathf.Abs(position.y) > halfSize;
    }

    // ���ֹͣ����
    private bool CheckStopCondition()
    {
        int maxRooms = 24; // ���Ը��ݸ����Ի�����������̬����
        return rooms.Count >= maxRooms;
    }

    // ���λ���Ƿ���Ч��û���ص���
    private bool IsValidPosition(Vector3 position)
    {
        return !Physics2D.OverlapCircle(position, 0.2f, roomLayer);
    }

    // ���÷����ǽ�ں���
    public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        float roomWidth = xOffset;
        float roomHeight = yOffset;

        // ������������Ƿ������ڷ���
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, roomHeight, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -roomHeight, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-roomWidth, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(roomWidth, 0, 0), 0.2f, roomLayer);

        newRoom.UpdateRoom(xOffset, yOffset); // ���·�������

        // �����ŵ��������ɶ�Ӧ��ǽ
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

    // Ѱ�ҽ�������
    public void FindEndRoom()
    {
        // ��ȡ�����
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;
        }

        // �ռ�������ʹδ����ķ���
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep)
                farRooms.Add(room.gameObject);
            if (room.stepToStart == maxStep - 1)
                lessFarRooms.Add(room.gameObject);
        }

        // ��Զ����ʹ�Զ�������ҳ�ֻ��һ���ŵķ���
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

        // ���ѡ��һ����Ϊ��������
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

// ����ǽ�����͵���
[System.Serializable]
public class WallType
{
    // ��ͬ���͵�ǽ��
    public GameObject singleLeft, singleRight, singleUp, singleBottom,
                      doubleUL, doubleLR, doubleBL, doubleUR, doubleUB, doubleBR,
                      tripleULR, tripleUBL, tripleUBR, tripleBLR,
                      fourDoors;
}
