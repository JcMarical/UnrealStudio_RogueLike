using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("������Ϣ")]
    public GameObject roomPrefab;
    public GameObject roomPrefab32_1; // �µĳ�����
    public GameObject roomPrefab16_2; // �µĿ���

    public int roomNumber;
    public Color startColor, endColor;
    private GameObject endRoom;

    [Header("λ�ÿ���")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;
    public LayerMask roomLayer;
    public int maxStep;

    public List<Room> rooms = new List<Room>();

    List<GameObject> farRooms = new List<GameObject>();

    List<GameObject> lessFarRooms = new List<GameObject>();

    List<GameObject> oneWayRooms = new List<GameObject>();

    private bool IsPreviousRoomSpecial = false;
    private GameObject lastRoomType = null; // ���ڴ洢��һ�����������
    public WallType wallType;

    void Start()
    {
        GenerafateRoom();
    }

    void GenerafateRoom()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            GameObject roomToInstantiate;

            if (i == 0 || IsPreviousRoomSpecial == true)   //��һ������/֮ǰ�ķ��������ⷿ�䣺��һ����������
            {
                roomToInstantiate = roomPrefab;
            }
            else    // �����һ���������������䣬��һ����ѡ
            {
                roomToInstantiate = GetRandomRoomPrefab();
            }


            rooms.Add(Instantiate(roomToInstantiate, generatorPoint.position, Quaternion.identity).GetComponent<Room>());   //����λ�ã����򣬻�ȡ��Room�������ӵ�room֮��

            IsPreviousRoomSpecial = roomToInstantiate == roomPrefab32_1 || roomToInstantiate == roomPrefab16_2;

            lastRoomType = roomToInstantiate;

            ChangePointPos(roomToInstantiate);
        }






        endRoom = rooms[0].gameObject;

        //�ҵ���󷿼�
        foreach (var room in rooms)
        {
            // if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)
            // {
            //    endRoom = room.gameObject;
            // }

            //SetupRoom(room, room.transform.position);
        }

        FindEndRoom();

        endRoom.GetComponent<SpriteRenderer>().color = endColor;

    }


    public void ChangePointPos(GameObject roomPrefab)
    {
        Vector3 currentPos = generatorPoint.position;

        float roomWidth = xOffset;
        float roomHeight = yOffset;

        // ������һ����������͵���ƫ����
        if (!IsPreviousRoomSpecial)
        {
            if (roomPrefab == roomPrefab32_1)
            {
                roomWidth *= 1.5f; // ��һ��������3.2:1���������
            }
            else if (roomPrefab == roomPrefab16_2)
            {
                roomHeight *= 1.5f; // ��һ��������1.6:2�������߶�
            }
        }
        else
        {
            // ��һ�����������ⷿ�䣬����Ϊ��ͨ����
            if (lastRoomType == roomPrefab32_1)
            {
                roomWidth *= 1.5f; // ��ǰ������3.2:1���������
            }
            else if (lastRoomType == roomPrefab16_2)
            {
                roomHeight *= 1.5f; // ��ǰ������1.6:2�������߶�
            }
        }


        do
        {
            direction = (Direction)Random.Range(0, 4);

            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position += new Vector3(0, roomHeight, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -roomHeight, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(-roomWidth, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(roomWidth, 0, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer)) ;

    }
    
    GameObject GetRandomRoomPrefab()    //������������ɲ�ͬ���͵ķ���,���ط����Ԥ����
    {
        int rand = Random.Range(0, 3);
        if (rand == 0) return roomPrefab;
        else if (rand == 1) return roomPrefab32_1;
        else return roomPrefab16_2;
    }

    /*public void SetupRoom(Room newRoom, Vector3 roomPosition)
    {
        float roomWidth = xOffset;
        float roomHeight = yOffset;

        if (newRoom.gameObject == roomPrefab32_1)
        {
            roomWidth *= 2;
        }
        else if (newRoom.gameObject == roomPrefab16_2)
        {
            roomHeight *= 2;
        }

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
    }*/

    public void FindEndRoom()
    {
        //�����ֵ
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;
        }

        //������ֵ����ʹδ�ֵ
        foreach(var room in rooms)
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

        if(oneWayRooms.Count != 0)
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
