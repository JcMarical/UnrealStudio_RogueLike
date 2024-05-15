using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData:MonoBehaviour
{
    public enum DamageKind{
        MeleeWeapon,
        RangedWeapon,
        TrapWeapon
    }
    public enum Weapon_specialEffect{
        None,
    }
    //挂载到武器profab并设置数值
        [Header("范围1~5由低到高")]
        public int rarity;//稀有度
        public int id;//武器编号
        public float DamageValue;//伤害
        public float AttachRadius;//攻击半径
        public float AttachInterval;//攻击间隔
        public DamageKind damageKind;//伤害类型
        public Weapon_specialEffect specialEffect;//特殊效果
}
