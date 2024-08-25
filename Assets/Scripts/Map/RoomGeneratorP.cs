using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGeneratorP : MonoBehaviour
{    
    // ���巽���ö��
    public enum Direction { up, down, left, right };
    public Direction direction;

    [Header("������Ϣ")]
    public GameObject[] roomPrefabs; // �����Ԥ�Ƽ�
    public float[] roomAreas; //ÿ����������

    public List<Vector3> positionUp = new List<Vector3>(); // �����ɷ����λ��
    public List<Vector3> positionDown = new List<Vector3>(); // �����ɷ����λ��
    public List<Vector3> positionLeft = new List<Vector3>(); // �����ɷ����λ��
    public List<Vector3> positionRight = new List<Vector3>(); // �����ɷ����λ��

    public GameObject initialRoom; //��ʼ��
    public GameObject theRoom; //�ո����ɵķ���

    [Header("λ�ÿ���")]
    public Transform generatorPoint; // ��ʼ���ɷ����λ��

    public LayerMask roomLayer;      // ����㣬���ڼ���ص�
    public int maxStep;              // ����㵽��ǰ����������
    public int step;
    public int roomCount;
    public int maxRoomCount;         // ��󷿼�����

    public Vector3 big; // ��������
    public Vector3 small; //С������
    private float area;  //С���������
    private float roomArea; //���ɷ������

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
        addPosition(transform.position, roomp, -1);
        //foreach (GameObject doorup in roomp.doorUp)
        //{
        //    Vector3 doorWorldPosition = theRoom.transform.position + doorup.transform.localPosition;
        //    positionUp.Add(doorWorldPosition);
        //}
        //foreach (GameObject doordown in roomp.doorUp)
        //{
        //    Vector3 doorWorldPosition = theRoom.transform.position + doordown.transform.localPosition;
        //    positionDown.Add(doorWorldPosition);
        //}
        //foreach (GameObject doorleft in roomp.doorUp)
        //{
        //    Vector3 doorWorldPosition = theRoom.transform.position + doorleft.transform.localPosition;
        //    positionLeft.Add(doorWorldPosition);
        //}
        //foreach (GameObject doorright in roomp.doorUp)
        //{
        //    Vector3 doorWorldPosition = theRoom.transform.position + doorright.transform.localPosition;
        //    positionRight.Add(doorWorldPosition);
        //}
        roomCount = 1;
        roomArea += CalculateTotalArea(theRoom);
        RoomGeneratorManager();
    }

    private void RoomGeneratorManager()
    {
        while (roomArea < 0.6 * area && step<maxStep && roomCount<maxRoomCount)
        {
            int direction= UnityEngine.Random.Range(0, 4);  //����,0 1 2 3�ֱ��Ӧ��������
            Debug.Log(direction);
            //Debug.Log("Position Up:");
            //LogList(positionUp);

            //Debug.Log("Position Down:");
            //LogList(positionDown);

            //Debug.Log("Position Left:");
            //LogList(positionLeft);

            //Debug.Log("Position Right:");
            //LogList(positionRight);
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
                    po = UnityEngine.Random.Range(0, positionUp.Count); //ѡ��λ��
                    ro= UnityEngine.Random.Range(0, roomPrefabs.Length); //ѡ�񷿼�
                    theRoom = roomPrefabs[ro];
                    roomp = theRoom.GetComponent<RoomP>();
                    if (roomp.doorDown.Length!=0)
                    {
                        //���ɷ�������
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
                    int po1 = UnityEngine.Random.Range(0, positionDown.Count); //ѡ��λ��
                    int ro1 = UnityEngine.Random.Range(0, roomPrefabs.Length); //ѡ�񷿼�
                    theRoom = roomPrefabs[ro1];
                    RoomP roomp1;
                    roomp1 = theRoom.GetComponent<RoomP>();
                    if (roomp1.doorUp.Length != 0)
                    {
                        //���ɷ�������
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
                    int po2 = UnityEngine.Random.Range(0, positionLeft.Count); //ѡ��λ��
                    int ro2 = UnityEngine.Random.Range(0, roomPrefabs.Length); //ѡ�񷿼�
                    theRoom = roomPrefabs[ro2];
                    RoomP roomp2;
                    roomp2 = theRoom.GetComponent<RoomP>();
                    if (roomp2.doorRight.Length != 0)
                    {
                        //���ɷ�������
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
                    int po3 = UnityEngine.Random.Range(0, positionRight.Count); //ѡ��λ��
                    int ro3 = UnityEngine.Random.Range(0, roomPrefabs.Length); //ѡ�񷿼�
                    theRoom = roomPrefabs[ro3];
                    RoomP roomp3;
                    roomp3 = theRoom.GetComponent<RoomP>();
                    if (roomp3.doorLeft.Length != 0)
                    {
                        //���ɷ�������
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
    }
    void addPosition(Vector3 position,RoomP room,int p)
    {
        int i;
        if (p!=1)
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
        if (p != 3)
        {
            for (i = 0; i < room.doorLeft.Length; i++)
            {
                positionLeft.Add(position + room.doorLeft[i].transform.localPosition);
            }
        }
        if (p != 2)
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

    // �������������ڴ�ӡ�б�����
    private void LogList(List<Vector3> list)
    {
        foreach (var vector in list)
        {
            Debug.Log(vector);
        }
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
