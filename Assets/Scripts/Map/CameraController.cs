using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // ����ģʽ��ʵ�����ã��������������з���
    public static CameraController instance;

    // ������ƶ����ٶ�
    public float speed;

    // �������ǰ�����Ŀ�����
    public Transform target;

    // �ڽű�ʵ����ʱ�����õ�������
    private void Awake()
    {
        instance = this;
    }

    // ��ÿ֡�����������λ�ã�ʹ�����ƶ���Ŀ������λ��
    void Update()
    {
        if (target != null)
        {
            // ����������ӵ�ǰλ���ƶ���Ŀ��λ�õĲ�ֵ������z�᲻��
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(target.position.x, target.position.y, transform.position.z),
                speed * Time.deltaTime
            );
        }
    }

    // ����������ĸ���Ŀ��
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
