using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISS
{
    #region �쳣״̬�����ӿ�
    public void SS_Hot(float harm) { }//���� ���������˺�
    public void SS_Freeze(float percent) { }//���� ����������������ӳ�ʱ�����
    public void SS_Fixation() { }//����
    public void SS_Confuse() { }//����
    public void SS_Sticky(float percent) { }//ճ�� �������������ٶȼ��ٱ���
    public void SS_Burn(float harm) { }//ȼ�� ���������˺�
    public void SS_Clog(float percent) { }//���� �������������ٶȼ��ٱ�����
    public void SS_Dizzy() { }//��ע
    public void SS_Hurry(float percent) { }//���� �������������ٶ����ӱ���
    public void SS_Blind(float radius) { }//��ä ����Ϊ����Բ�İ뾶
    public void SS_Charm(Transform target, float speed) { }//�Ȼ� ��һ������Ϊ�������쳣Ч�������λ�ã��ڶ���Ϊ������������ƶ�ʱ���ٶ�
    public void SS_Invincible() { }//�޵�
    #endregion
}

