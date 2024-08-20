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
    // ���巽���ö��
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("������Ϣ")]
    public GameObject[] roomPrefabs; // �����Ԥ�Ƽ�
    public float[] roomAreas; //ÿ����������

    private List<Vector3> positionUp = new List<Vector3>(); // �����ɷ����λ��
    private List<Vector3> positionDown = new List<Vector3>(); // �����ɷ����λ��
    private List<Vector3> positionLeft = new List<Vector3>(); // �����ɷ����λ��
    private List<Vector3> positionRight = new List<Vector3>(); // �����ɷ����λ��

    public GameObject initialRoom; //��ʼ��
    public GameObject theRoom; //�ո����ɵķ���

    [Header("λ�ÿ���")]
    public Transform generatorPoint; // ��ʼ���ɷ����λ��

    public LayerMask roomLayer;      // ����㣬���ڼ���ص�
    public int maxStep;              // ����㵽��ǰ����������
    public int maxRoomCount;         // ��󷿼�����

    public Vector3 big; // ��������
    public Vector3 small; //С������
    private float area;  //С���������
    private float roomArea; //���ɷ������
    private bool stop=false;

    private void OnDrawGizmosSelected()
    {
        // �� Unity �༭���л��ƴ�С�����Σ�ʹ�õ�ǰ�����λ����Ϊ���ĵ�
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, small);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, big);
    }

    void Start()
    {
        //�����������
        area = small.x * small.y;
        int i = 0;
        foreach (GameObject prefab in roomPrefabs)
        {
            roomAreas[i] = CalculateTotalArea(prefab);
            i++;
        }

        //���ɳ�ʼ�źͳ�ʼ��
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
            int direction= UnityEngine.Random.Range(0, 4);  //����,0 1 2 3�ֱ��Ӧ��������
            switch (direction)
            {
                case 0:
                    if (positionUp.Count == 0)
                    {
                        break;
                    }
                    int po = UnityEngine.Random.Range(0, positionUp.Count); //ѡ��λ��
                    int ro= UnityEngine.Random.Range(0, roomPrefabs.Length); //ѡ�񷿼�
                    theRoom = roomPrefabs[ro];
                    RoomP roomp;
                    roomp = theRoom.GetComponent<RoomP>();
                    if (roomp.doorDown.Length!=0)
                    {
                        //���ɷ�������
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
        // ��ȡԤ�Ƽ�������������
        Collider2D[] colliders = prefab.GetComponentsInChildren<Collider2D>();
        Bounds totalBounds = new Bounds(prefab.transform.position, Vector3.zero);

        foreach (Collider2D collider in colliders)
        {
            // ��չ�߽��԰�������������
            totalBounds.Encapsulate(collider.bounds);
        }

        // �������
        float width = totalBounds.size.x;
        float height = totalBounds.size.y;
        float area1 = width * height;

        return area1;
    }

    // ���λ���Ƿ���Ч��û���ص���
    private bool IsValidPosition(Vector3 position)
    {
        return !Physics2D.OverlapCircle(position, 0.2f, roomLayer);
    }
}

//// ����ǽ�����͵���
//[System.Serializable]
//public class WallType
//{
//    // ��ͬ���͵�ǽ��
//    public GameObject singleLeft, singleRight, singleUp, singleBottom,
//                      doubleUL, doubleLR, doubleBL, doubleUR, doubleUB, doubleBR,
//                      tripleULR, tripleUBL, tripleUBR, tripleBLR,
//                      fourDoors;
//}
