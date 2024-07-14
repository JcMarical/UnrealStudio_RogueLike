using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct WeaponData
{
    //武器数据结构体
        public int rarity;//稀有度
        public int id;//武器编号
        public float DamageValue;//伤害
        public float AttachRadius;//攻击半径
        public float AttachInterval;//攻击间隔
        public DamageKind damageKind;//伤害类型
        public Weapon_EffectType specialEffect;//特殊效果
        public float RepelAbility;//击退能力
        public float MaxPower;//满能量数值
        public float CurrentPower;//当前能量
        public float DefaultCharge_Value;//默认单次攻击充能量
        public Sprite sprite;//武器图片
        public int segment;//攻击段数
        public List<Collider2D> Range;//武器不同段攻击的不同碰撞箱
}
