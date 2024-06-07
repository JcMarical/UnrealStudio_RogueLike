using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器动画机控制抽象类
/// </summary>
public abstract class WeaponAnimCtrl : MonoBehaviour
{
    protected Animator ThisAnimator;
    private void Awake() {
        ThisAnimator = GetComponent<Animator>();
    }
    /// <summary>
    /// 攻击时调用
    /// </summary>
    public abstract void OnAttack();
}
