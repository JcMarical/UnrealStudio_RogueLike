using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器动画机控制抽象类
/// </summary>
public abstract class WeaponAnimCtrl : MonoBehaviour
{
    private Animator ThisAnimator;
    public Action EndEvent;
    private bool m_Continue;
    protected bool Continue{
        get{ return m_Continue; }
        set{
            m_Continue=value;
            ThisAnimator.SetBool("Continue",value);
        }
    }
    private void Awake() {
        ThisAnimator = GetComponent<Animator>();
    }
    /// <summary>
    /// 攻击时调用
    /// </summary>
    public abstract void OnAttack();
    public abstract void AnimBegin();
    public abstract void AnimEnd();
}
