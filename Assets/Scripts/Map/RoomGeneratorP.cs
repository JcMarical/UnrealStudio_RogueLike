using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGeneratorP : MonoBehaviour
{    
    // 定义方向的枚举
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("房间信息")]
    public GameObject[] roomPrefabs; // 房间的预制件
    public float[] roomAreas; //每个房间的面积

    public List<Vector3> positionUp = new List<Vector3>(); // 可生成房间的位置
    public List<Vector3> positionDown = new List<Vector3>(); // 可生成房间的位置
    public List<Vector3> positionLeft = new List<Vector3>(); // 可生成房间的位置
    public List<Vector3> positionRight = new List<Vector3>(); // 可生成房间的位置
    List<Vector3> positionUpInitial = new List<Vector3>();
    List<Vector3> positionDownInitial = new List<Vector3>();
    List<Vector3> positionLeftInitial = new List<Vector3>();
    List<Vector3> positionRightInitial = new List<Vector3>();
    public GameObject initialRoom; //初始房
    public GameObject theRoom; //刚刚生成的房间

    [Header("位置控制")]
    public Transform generatorPoint; // 初始生成房间的位置

    public LayerMask roomLayer;      // 房间层，用于检测重叠
    public int maxStep;              // 从起点到当前房间的最大步数
    public int step;
    public int roomCount;
    public int maxRoomCount;         // 最大房间数量

    public Vector3 big; // 大正方形
    public Vector3 small; //小正方形
    private float area; //小正方形面积
    private float roomArea; //生成房间面积
    bool isOut; //出大正方形

    private void OnDrawGizmosSelected()
    {
        // 在 Unity 编辑器中绘制大小正方形，使用当前物体的位置作为中心点
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, small);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, big);
    }

    void Start()
    {
        //计算和填充面积
        area = small.x * small.y;
        int i = 0;
        foreach (GameObject prefab in roomPrefabs)
        {
            roomAreas[i] = CalculateTotalArea(prefab);
            i++;
        }

        //生成初始门和初始化
        theRoom=Instantiate(initialRoom,transform.position,Quaternion.identity);
        RoomP roomp = theRoom.GetComponent<RoomP>();
        addPosition(transform.position, roomp, -1);
        isOut = false;
        positionUpInitial = new List<Vector3>(positionUp);positionDownInitial = new List<Vector3>(positionDown);
        positionLeftInitial = new List<Vector3>(positionLeft);positionRightInitial = new List<Vector3>(positionRight);

        RoomGeneratorManagerBefore();
        step = 0;

        //positionUp = new List<Vector3>(positionUpInitial);positionDown = new List<Vector3>(positionDownInitial);
        //positionLeft = new List<Vector3>(positionLeftInitial);positionRight = new List<Vector3>(positionRightInitial);
        positionUp = GetHalfElements(positionUpInitial);
        positionDown = GetHalfElements(positionDownInitial);
        positionLeft = GetHalfElements(positionLeftInitial);
        positionRight = GetHalfElements(positionRightInitial);

        roomCount = 1;
        roomArea += CalculateTotalArea(theRoom);
        RoomGeneratorManager();
    }

    List<Vector3> GetHalfElements(List<Vector3> originalList)
    {
        // 确保列表不为空
        if (originalList == null || originalList.Count == 0)
        {
            return new List<Vector3>();
        }

        // 复制原始列表
        List<Vector3> newList = new List<Vector3>(originalList);
        int halfCount = Mathf.CeilToInt(newList.Count / 2f); // 计算一半的元素数量

        // 打乱列表
        newList = newList.OrderBy(x => Random.value).ToList();

        // 保留一半的元素
        return newList.Take(halfCount).ToList();
    }
    private void RoomGeneratorManagerBefore()
    {
        int direction = UnityEngine.Random.Range(0, 4);  //方向,0 1 2 3分别对应上下左右
        while (step<maxStep && !isOut)
        {
            switch (direction)
            {
                case 0:
                    GenerateRoom(positionUp, 0, room => room.doorDown, room => room.doorUp, Vector3.up);
                    break;
                case 1:
                    GenerateRoom(positionDown, 1, room => room.doorUp, room => room.doorDown, Vector3.down);
                    break;
                case 2:
                    GenerateRoom(positionLeft, 2, room => room.doorRight, room => room.doorLeft, Vector3.left);
                    break;
                case 3:
                    GenerateRoom(positionRight, 3, room => room.doorLeft, room => room.doorRight, Vector3.right);
                    break;
            }
        }
    }
    private void RoomGeneratorManager()
    {
        while (roomArea < 0.6f * area && step<maxStep && roomCount<maxRoomCount)
        {
            int direction= UnityEngine.Random.Range(0, 4);  //方向,0 1 2 3分别对应上下左右
            Debug.Log(direction);
            switch (direction)
            {
                case 0:
                    GenerateRoom(positionUp, 0, room => room.doorDown, room => room.doorUp, Vector3.up);
                    break;
                case 1:
                    GenerateRoom(positionDown, 1, room => room.doorUp, room => room.doorDown, Vector3.down);
                    break;
                case 2:
                    GenerateRoom(positionLeft, 2, room => room.doorRight, room => room.doorLeft, Vector3.left);
                    break;
                case 3:
                    GenerateRoom(positionRight, 3, room => room.doorLeft, room => room.doorRight, Vector3.right);
                    break;
            }
        }
        /*
        switch (direction)
            {

                case 0:
                    if (positionUp.Count == 0)
                    {
                        Debug.Log("count0");
                        break;

                    }
                    int po;
                    int ro;
                    RoomP roomp;
                    po = UnityEngine.Random.Range(0, positionUp.Count); //选择位置
                    ro= UnityEngine.Random.Range(0, roomPrefabs.Length); //选择房间
                    theRoom = roomPrefabs[ro];
                    roomp = theRoom.GetComponent<RoomP>();
                    if (roomp.doorDown.Length!=0)
                    {
                        //生成房间坐标
                        Vector3 newPosition = positionUp[po] - roomp.doorDown[UnityEngine.Random.Range
                            (0, roomp.doorDown.Length)].transform.localPosition;
                        //if (!Physics2D.OverlapCircle(newPosition, 0.2f, roomLayer) && IsValidPosition(newPosition))
                        //{
                            if (roomp.doorUp.Length!=0 && (newPosition + roomp.doorUp[0].transform.localPosition).y
                                <transform.position.y+big.y/2)
                            {
                                Debug.Log("Crossed");
                                break;
                            }
                            Instantiate(theRoom,newPosition, Quaternion.identity);
                            positionUp.RemoveAt(po);
                            addPosition(newPosition, roomp,0);
                            roomArea += CalculateTotalArea(theRoom);
                            roomCount++;
                            break;
                        //}
                        //Debug.Log("Collider");
                        //break;
                    }
                    Debug.Log("door0");
                    break; 
                case 1:
                    if (positionDown.Count == 0)
                    {
                        Debug.Log("count0");
                        break;
                    }
                    int po1 = UnityEngine.Random.Range(0, positionDown.Count); //选择位置
                    int ro1 = UnityEngine.Random.Range(0, roomPrefabs.Length); //选择房间
                    theRoom = roomPrefabs[ro1];
                    RoomP roomp1;
                    roomp1 = theRoom.GetComponent<RoomP>();
                    if (roomp1.doorUp.Length != 0)
                    {
                        //生成房间坐标
                        Vector3 newPosition = positionDown[po1] - roomp1.doorUp[UnityEngine.Random.Range
                            (0, roomp1.doorDown.Length)].transform.localPosition;
                        //if (!Physics2D.OverlapCircle(newPosition, 0.2f, roomLayer) && IsValidPosition(newPosition))
                        //{
                            if (roomp1.doorDown.Length != 0 && (newPosition + roomp1.doorDown[0].transform.localPosition).y
                                < (transform.position.y - big.y / 2))
                            {
                                Debug.Log("Crossed");
                                break;
                            }
                            Instantiate(theRoom, newPosition, Quaternion.identity);
                            positionDown.RemoveAt(po1);
                            addPosition(newPosition, roomp1, 1);
                            roomArea += CalculateTotalArea(theRoom);
                            roomCount++;
                            break;
                        //}
                        //Debug.Log("Collider");
                        //break;
                    }
                    Debug.Log("door0");
                    break;
                case 2:
                    if (positionLeft.Count == 0)
                    {
                        Debug.Log("count0");
                        break;
                    }
                    int po2 = UnityEngine.Random.Range(0, positionLeft.Count); //选择位置
                    int ro2 = UnityEngine.Random.Range(0, roomPrefabs.Length); //选择房间
                    theRoom = roomPrefabs[ro2];
                    RoomP roomp2;
                    roomp2 = theRoom.GetComponent<RoomP>();
                    if (roomp2.doorRight.Length != 0)
                    {
                        //生成房间坐标
                        Vector3 newPosition = positionLeft[po2] - roomp2.doorRight[UnityEngine.Random.Range
                            (0, roomp2.doorDown.Length)].transform.localPosition;
                        //if (!Physics2D.OverlapCircle(newPosition, 0.2f, roomLayer) && IsValidPosition(newPosition))
                        //{
                            if (roomp2.doorLeft.Length != 0 && (newPosition + roomp2.doorLeft[0].transform.localPosition).x
                                < (transform.position.x - big.x / 2))
                            {
                                Debug.Log("Crossed");
                                break;
                            }
                            Instantiate(theRoom, newPosition, Quaternion.identity);
                            positionLeft.RemoveAt(po2);
                            addPosition(newPosition, roomp2, 2);
                            roomArea += CalculateTotalArea(theRoom);
                            roomCount++;
                            break;
                        //}
                        //Debug.Log("Collider");
                        //break;
                    }
                    Debug.Log("door0");
                    break;
                case 3:
                    if (positionRight.Count == 0)
                    {
                        Debug.Log("count0");
                        break;
                    }
                    int po3 = UnityEngine.Random.Range(0, positionRight.Count); //选择位置
                    int ro3 = UnityEngine.Random.Range(0, roomPrefabs.Length); //选择房间
                    theRoom = roomPrefabs[ro3];
                    RoomP roomp3;
                    roomp3 = theRoom.GetComponent<RoomP>();
                    if (roomp3.doorLeft.Length != 0)
                    {
                        //生成房间坐标
                        Vector3 newPosition = positionRight[po3] - roomp3.doorLeft[UnityEngine.Random.Range
                            (0, roomp3.doorDown.Length)].transform.localPosition;
                        //if (!Physics2D.OverlapCircle(newPosition, 0.2f, roomLayer) && IsValidPosition(newPosition))
                        //{
                            if (roomp3.doorRight.Length != 0 && (newPosition + roomp3.doorRight[0].transform.localPosition).x
                                > (transform.position.x + big.x / 2))
                            {
                                Debug.Log("Crossed");
                                break;
                            }
                            Instantiate(theRoom, newPosition, Quaternion.identity);
                            positionRight.RemoveAt(po3);
                            addPosition(newPosition, roomp3, 3);
                            roomArea += CalculateTotalArea(theRoom);
                            roomCount++;
                            break;
                        //}
                        //Debug.Log("Collider");
                        //break;
                    }
                    Debug.Log("door0");
                    break;
            }
        }
        */
    }
    private void GenerateRoom(List<Vector3> positionList, int directionIndex,
       System.Func<RoomP, GameObject[]> getOppositeDoors,
       System.Func<RoomP, GameObject[]> getCurrentDoors, Vector3 directionVector)
    {
        if (positionList.Count == 0)
        {
            Debug.Log("No available positions");
            return;
        }

        int po = UnityEngine.Random.Range(0, positionList.Count);
        int ro = UnityEngine.Random.Range(0, roomPrefabs.Length);
        theRoom = roomPrefabs[ro];
        RoomP roomp = theRoom.GetComponent<RoomP>();

        if (getOppositeDoors(roomp).Length != 0)
        {
            Vector3 newPosition = positionList[po] - getOppositeDoors(roomp)[UnityEngine.Random.Range(0, getOppositeDoors(roomp).Length)].transform.localPosition;

            // 超出大正方形检测
            if (!IsWithinBounds(newPosition, getCurrentDoors(roomp)[0].transform.localPosition, directionVector))
            {
                Debug.Log("Out of bounds");
                isOut = true;
                return;
            }

            // 重叠检测
            if (IsValidPosition(newPosition))
            {
                Instantiate(theRoom, newPosition, Quaternion.identity);
                positionList.RemoveAt(po);
                addPosition(newPosition, roomp, directionIndex);
                roomArea += CalculateTotalArea(theRoom);
                roomCount++;
            }
            else
            {
                Debug.Log("Position overlaps with existing room");
            }
        }
        else
        {
            Debug.Log("No doors in opposite direction");
        }
    }

    void addPosition(Vector3 position,RoomP room,int p)
    {
        if (p!=1)
        {
            foreach(var door in room.doorUp)
            {
                positionUp.Add(position + door.transform.localPosition);
            }
        }
        if (p!=0)
        {
            foreach (var door in room.doorDown)
            {
                positionDown.Add(position + door.transform.localPosition);
            }
        }
        if (p != 3)
        {
            foreach (var door in room.doorLeft)
            {
                positionLeft.Add(position + door.transform.localPosition);
            }
        }
        if (p != 2)
        {
            foreach (var door in room.doorRight)
            {
                positionRight.Add(position + door.transform.localPosition);
            }
        }
    }

    private bool IsWithinBounds(Vector3 position, Vector3 doorPosition, Vector3 direction)
    {
        Vector3 checkPosition = position + doorPosition;

        if (direction == Vector3.up || direction == Vector3.down)
        {
            return checkPosition.y < transform.position.y + big.y / 2 && checkPosition.y > transform.position.y - big.y / 2;
        }
        else if (direction == Vector3.left || direction == Vector3.right)
        {
            return checkPosition.x < transform.position.x + big.x / 2 && checkPosition.x > transform.position.x - big.x / 2;
        }

        return true;
    }

    float CalculateTotalArea(GameObject prefab)
    {
        // 获取预制件的所有子物体
        Collider2D[] colliders = prefab.GetComponentsInChildren<Collider2D>();
        Bounds totalBounds = new Bounds(prefab.transform.position, Vector3.zero);

        foreach (Collider2D collider in colliders)
        {
            // 扩展边界以包括所有子物体
            totalBounds.Encapsulate(collider.bounds);
        }

        return totalBounds.size.x * totalBounds.size.y;
    }

    // 检查位置是否有效（没有重叠）
    private bool IsValidPosition(Vector3 position)
    {
        return !Physics2D.OverlapCircle(position, 0.2f, roomLayer);
    }

    // 辅助方法，用于打印列表内容
    private void LogList(List<Vector3> list)
    {
        foreach (var vector in list)
        {
            Debug.Log(vector);
        }
    }

}

//// 定义墙体类型的类
//[System.Serializable]
//public class WallType
//{
//    // 不同类型的墙体
//    public GameObject singleLeft, singleRight, singleUp, singleBottom,
//                      doubleUL, doubleLR, doubleBL, doubleUR, doubleUB, doubleBR,
//                      tripleULR, tripleUBL, tripleUBR, tripleBLR,
//                      fourDoors;
//}
