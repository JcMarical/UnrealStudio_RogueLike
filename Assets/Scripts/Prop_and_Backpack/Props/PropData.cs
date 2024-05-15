using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="PropData",menuName ="Data/Props/Data",order =1)]
public class PropData : ScriptableObject
{
    /// <summary>
    /// ����ϡ�жȣ���N��URϡ�ж����ε�����N��Ӧ1���Դ�����
    /// </summary>
    public enum Rarities
    { 
        N =1,
        R,
        SR,
        SSR,
        UR
    }

    /// <summary>
    /// ������
    /// </summary>
    public string PropName;

    /// <summary>
    /// ���߱��
    /// </summary>
    public int PropID;

    /// <summary>
    /// ����ͼ��
    /// </summary>
    [HideInInspector]public Sprite PropIcon;

    /// <summary>
    /// ����ϡ�ж�
    /// </summary>
    public Rarities Rarity;

    /// <summary>
    /// �����Ƿ�Ϊ��������
    /// </summary>
    public bool Consumable;

    /// <summary>
    /// ���߹��ܽ���
    /// </summary>
    [TextArea]public string PropDesc;

    /// <summary>
    /// ���߻�ȡ����
    /// </summary>
    [TextArea]public string WaytoGet;

    /// <summary>
    /// ����˵�����²�ʽ�Ľ���
    /// </summary>
    [TextArea]public string OtherDesc;

    /// <summary>
    /// ���߹��ܺ���
    /// </summary>
    public PropFunc PropFunc;
}
