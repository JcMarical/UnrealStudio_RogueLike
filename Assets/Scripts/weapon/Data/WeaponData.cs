using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
[Serializable]
public struct specialEffect_Weapon{
    public SpecialState.TargetType targetType;
    public float Duration;
}
[Serializable]
[CreateAssetMenu(menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject, ITradable
{
        //武器数据结构体，_bas为初始值，_fac为实际值
        #region 属性
        public int rarity;//稀有度
        public int id;//武器编号
        public int value;//价值
        public DamageKind damageKind;//伤害类型
        public List<specialEffect_Weapon> specialEffect;//特殊效果 
        public Sprite sprite;//武器图片
        public CircleCollider2D Range;//武器不同段攻击的不同碰撞箱
        #endregion

        #region 基础数值
        public float DamageValue_bas;//伤害
        public float AttackRadius_bas;//攻击半径
        public float AttackInterval_bas;//攻击间隔
        public float MaxPower_bas;//满能量数值
        public float ExpulsionStrength;//击退力度

    #endregion

    public int Price { get; set; }

    public void BeBought(Transform transform)
    {
        //TODO:处理动画效果
    }

    public void BeSoldOut()
    {
        //TODO:处理动画效果
    }
}
