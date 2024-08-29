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
        // 延迟一帧，以确保敌人已经完全生成并位于场景中
        Invoke(nameof(CheckCollision), 0.1f);
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
        int halfCount = Mathf.CeilToInt(newList.Count / 1.5f); // 计算一半的元素数量

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
            step++;
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
            step += 1;
            int direction= UnityEngine.Random.Range(0, 4);  //方向,0 1 2 3分别对应上下左右
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
    private void GenerateRoom(List<Vector3> positionList, int directionIndex,
       System.Func<RoomP, GameObject[]> getOppositeDoors,
       System.Func<RoomP, GameObject[]> getCurrentDoors, Vector3 directionVector)
    {
        if (positionList.Count == 0)
        {
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
                isOut = true;
                return;
            }

            // 重叠检测
            if (IsValidPosition(newPosition))
            {
                GameObject instantiatedRoom = Instantiate(theRoom, newPosition, Quaternion.identity);

                if (CheckCollision(instantiatedRoom))
                {
                    //// 如果有碰撞，销毁房间
                    //Debug.Log("Collision detected, destroying room");
                    Debug.Log(1);
                    Destroy(instantiatedRoom);
                }
                else
                {
                    // 如果没有碰撞，保留房间
                    positionList.RemoveAt(po);
                    addPosition(newPosition, roomp, directionIndex);
                    roomArea += CalculateTotalArea(instantiatedRoom);
                    roomCount++;
                }
            }
            //else
            //{
            //    Debug.Log("Position overlaps with existing room");
            //}
        }
        else
        {
            Debug.Log("No doors in opposite direction");
        }
    }
    private bool CheckCollision(GameObject room)
    {
        // 获取所有子物体上的Collider2D组件
        Collider2D[] colliders = room.GetComponentsInChildren<Collider2D>();

        // 遍历每一个Collider2D
        foreach (Collider2D collider in colliders)
        {
            Bounds bounds = collider.bounds;

            //所有碰撞体
            Collider2D[] overlapColliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);

            foreach (Collider2D overlapCollider in overlapColliders)
            {
                // 检查检测到的碰撞体是否与当前遍历的碰撞体相同，如果相同则跳过（防止自我检测）
                if (overlapCollider == collider) continue;

                // 检查检测到的碰撞体的物体是否在指定层上
                //Debug.Log(overlapCollider.gameObject.layer);
                //Debug.Log(roomLayer.value);
                if (overlapCollider.gameObject.layer == 8)
                {
                    return true;
                }
            }
        }
        return false;
        //Collider2D roomCollider = room.GetComponent<Collider2D>();
        //if (roomCollider != null)
        //{
        //    Collider2D[] colliders = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f);
        //    foreach (Collider2D collider in colliders)
        //    {
        //        if (collider.gameObject.layer == roomLayer)
        //        {
        //            return true; // 退出方法
        //        }
        //    }
        //}
        //return false;
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
