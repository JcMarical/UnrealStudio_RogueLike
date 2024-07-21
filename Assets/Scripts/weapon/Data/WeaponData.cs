using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public readonly struct WeaponData
{
        //武器数据结构体，_bas为初始值，_fac为实际值
        #region 属性
        public readonly int rarity;//稀有度
        public readonly int id;//武器编号
        public readonly DamageKind damageKind;//伤害类型
        public readonly Weapon_EffectType specialEffect;//特殊效果 
        public readonly Sprite sprite;//武器图片
        public readonly int segment;//攻击段数
        public readonly List<Collider2D> Range;//武器不同段攻击的不同碰撞箱
        #endregion

        #region 基础数值
        public readonly float DamageValue_bas;//伤害
        public readonly float AttachRadius_bas;//攻击半径
        public readonly float AttachInterval_bas;//攻击间隔
        public readonly float MaxPower_bas;//满能量数值
        public readonly float Weight_bas;//武器重量
        public readonly float ExpulsionStrength;//击退力度
        public readonly float DefaultCharge;
        #endregion    
}
