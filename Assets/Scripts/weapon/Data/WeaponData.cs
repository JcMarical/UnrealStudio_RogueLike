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
        public GameObject Range;
}
