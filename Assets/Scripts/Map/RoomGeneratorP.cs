using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class RoomGeneratorP : MonoBehaviour
{    
    // 定义方向的枚举
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("房间信息")]
    public GameObject[] roomPrefabs; // 房间的预制件
    public float[] roomAreas; //每个房间的面积

    private List<Vector3> positionUp = new List<Vector3>(); // 可生成房间的位置
    private List<Vector3> positionDown = new List<Vector3>(); // 可生成房间的位置
    private List<Vector3> positionLeft = new List<Vector3>(); // 可生成房间的位置
    private List<Vector3> positionRight = new List<Vector3>(); // 可生成房间的位置

    public GameObject initialRoom; //初始房
    public GameObject theRoom; //刚刚生成的房间

    [Header("位置控制")]
    public Transform generatorPoint; // 初始生成房间的位置

    public LayerMask roomLayer;      // 房间层，用于检测重叠
    public int maxStep;              // 从起点到当前房间的最大步数
    public int maxRoomCount;         // 最大房间数量

    public Vector3 big; // 大正方形
    public Vector3 small; //小正方形
    private float area;  //小正方形面积
    private float roomArea; //生成房间面积
    private bool stop=false;

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
        RoomP roomp;
        roomp=theRoom.GetComponent<RoomP>();
        foreach (GameObject doorup in roomp.doorUp)
        {
            Vector3 doorWorldPosition = theRoom.transform.position + doorup.transform.localPosition;
            positionUp.Add(doorWorldPosition);
        }
        foreach (GameObject doordown in roomp.doorUp)
        {
            Vector3 doorWorldPosition = theRoom.transform.position + doordown.transform.localPosition;
            positionDown.Add(doorWorldPosition);
        }
        foreach (GameObject doorleft in roomp.doorUp)
        {
            Vector3 doorWorldPosition = theRoom.transform.position + doorleft.transform.localPosition;
            positionLeft.Add(doorWorldPosition);
        }
        foreach (GameObject doorright in roomp.doorUp)
        {
            Vector3 doorWorldPosition = theRoom.transform.position + doorright.transform.localPosition;
            positionRight.Add(doorWorldPosition);
        }
    }

    private void RoomGeneratorManager()
    {
        while (roomArea < 0.6 * area)
        {
            int direction= UnityEngine.Random.Range(0, 4);  //方向,0 1 2 3分别对应上下左右
            switch (direction)
            {
                case 0:
                    if (positionUp.Count == 0)
                    {
                        break;
                    }
                    int po = UnityEngine.Random.Range(0, positionUp.Count); //选择位置
                    int ro= UnityEngine.Random.Range(0, roomPrefabs.Length); //选择房间
                    theRoom = roomPrefabs[ro];
                    RoomP roomp;
                    roomp = theRoom.GetComponent<RoomP>();
                    if (roomp.doorDown.Length!=0)
                    {
                        //生成房间坐标
                        Vector3 newPosition = positionUp[po] + roomp.doorDown[UnityEngine.Random.Range
                            (0, roomp.doorDown.Length)].transform.localPosition;
                        if (!Physics2D.OverlapCircle(newPosition, 0.2f, roomLayer) && IsValidPosition(newPosition))
                        {
                            if (roomp.doorUp.Length!=0 && (newPosition + roomp.doorUp[0].transform.localPosition).y
                                <transform.position.y+big.y/2)
                            {
                                break;
                            }
                            Instantiate(theRoom,newPosition, Quaternion.identity);
                            positionUp.RemoveAt(po);
                            addPosition(newPosition, roomp,0);
                        }
                    }
                    break; 
                case 1:
                    break; 
                case 2:
                    break;
                case 3:
                    break;
            }
        }
    }
    void addPosition(Vector3 position,RoomP room,int p)
    {
        int i;
        if (p!=-1)
        {
            for (i = 0; i < room.doorUp.Length; i++)
            {
                positionUp.Add(position + room.doorUp[i].transform.localPosition);
            }
        }
        if (p!=0)
        {
            for (i = 0; i < room.doorDown.Length; i++)
            {
                positionDown.Add(position + room.doorDown[i].transform.localPosition);
            }
        }
        if (p != -1)
        {
            for (i = 0; i < room.doorLeft.Length; i++)
            {
                positionLeft.Add(position + room.doorLeft[i].transform.localPosition);
            }
        }
        if (p != -1)
        {
            for (i = 0; i < room.doorRight.Length; i++)
            {
                positionRight.Add(position + room.doorRight[i].transform.localPosition);
            }
        }
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

        // 计算面积
        float width = totalBounds.size.x;
        float height = totalBounds.size.y;
        float area1 = width * height;

        return area1;
    }

    // 检查位置是否有效（没有重叠）
    private bool IsValidPosition(Vector3 position)
    {
        return !Physics2D.OverlapCircle(position, 0.2f, roomLayer);
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
