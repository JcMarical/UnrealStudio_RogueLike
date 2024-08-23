using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    // ������ĸ��ŵ�GameObject����
    public GameObject doorLeft, doorRight, doorUp, doorDown;

    // �����Ƿ��ж�Ӧ�������
    public bool roomLeft, roomRight, roomUp, roomDown;

    // ����㵽�˷���Ĳ���
    public int stepToStart;

    // ��ʾ�������ı�UI
    public Text text;

    // �����ŵ�����
    public int doorNumber;

    // ��ʼ�����䣬�����ŵļ���״̬
    void Start()
    {
        doorLeft.SetActive(roomLeft);    // ������������
        doorRight.SetActive(roomRight);  // ������������
        doorUp.SetActive(roomUp);        // ������������
        doorDown.SetActive(roomDown);    // ������������
    }

    // ���·���״̬���������㲽�����ŵ�����
    public void UpdateRoom(float xOffset, float yOffset)
    {
        // �������㵽�˷���Ĳ���
        stepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));
        text.text = stepToStart.ToString(); // ��UI����ʾ����

        // �����Ƿ������������ŵ�����
        if (roomUp)
            doorNumber++;
        if (roomDown)
            doorNumber++;
        if (roomLeft)
            doorNumber++;
        if (roomRight)
            doorNumber++;
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
