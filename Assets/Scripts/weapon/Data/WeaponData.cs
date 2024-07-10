using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct WeaponData
{
    //武器数据结构体，_bas为初始值，_fac为实际值
    #region 属性
        public int rarity;//稀有度
        public int id;//武器编号
        public DamageKind damageKind;//伤害类型
        public Weapon_EffectType specialEffect;//特殊效果 
        public Sprite sprite;//武器图片
        public int segment;//攻击段数
        public List<Collider2D> Range;//武器不同段攻击的不同碰撞箱
        #endregion

        #region 基础数值
        public float DamageValue_bas;//伤害
        public float AttachRadius_bas;//攻击半径
        public float AttachInterval_bas;//攻击间隔
        public float RepelAbility_bas;//击退能力
        public float MaxPower_bas;//满能量数值
        public float Weight_bas;//武器重量
        #endregion

        #region 实际数值
        public float DamageValue_fac;//伤害
        public float AttachRadius_fac;//攻击半径
        public float AttachInterval_fac;//攻击间隔
        public float RepelAbility_fac;//击退能力
        public float Power_fac;//满能量数值
        public float DefaultCharge_Value;//默认单次攻击充能量
        public float Weight_fac;//武器重量
       #endregion
        
}
