using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerEasy : MonoBehaviour
{
    public float speed = 5f; // ��������ƶ��ٶ�

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = Vector3.zero; // ��ʼ���ƶ�����
        if (Input.GetKey(KeyCode.UpArrow)) // ��������ϼ�ͷ��
        {
            moveDir += Vector3.up; // �����ƶ�
        }
        if (Input.GetKey(KeyCode.DownArrow)) // ��������¼�ͷ��
        {
            moveDir += Vector3.down; // �����ƶ�
        }
        if (Input.GetKey(KeyCode.LeftArrow)) // ����������ͷ��
        {
            moveDir += Vector3.left; // �����ƶ�
        }
        if (Input.GetKey(KeyCode.RightArrow)) // ��������Ҽ�ͷ��
        {
            moveDir += Vector3.right; // �����ƶ�
        }

        // ����������ٶȸı�λ��
        transform.position += moveDir.normalized * speed * Time.deltaTime;
    }
}