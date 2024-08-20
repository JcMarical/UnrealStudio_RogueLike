using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomP : MonoBehaviour
{
    // ������ĸ��ŵ�GameObject����
    public GameObject[] doorLeft, doorRight, doorUp, doorDown;
    // �����Ƿ��ж�Ӧ�������
    public bool roomLeft, roomRight, roomUp, roomDown;

    // ��ʼ�����䣬�����ŵļ���״̬
    void Start()
    {

    }

    // ������ײ�¼�ʱ���л��������Ŀ�굽��ǰ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.instance.ChangeTarget(transform);
        }
    }
}
