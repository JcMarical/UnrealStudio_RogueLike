using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Enums;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
[Serializable]
public struct specialEffect_Weapon{
    public SpecialState_Type targetType;
    public float Duration;
}
[Serializable]
[CreateAssetMenu(menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject, ITradable 
{
        //武器数据结构体，_bas为初始值，_fac为实际值
        #region 属性
        public Rarities rarity;//稀有度
        public int id;//武器编号
        public int value;//价值
        public DamageKind damageKind;//伤害类型
        public List<specialEffect_Weapon> specialEffect;//特殊效果 
        public Sprite sprite;//武器图片
        public CircleCollider2D Range;//武器不同段攻击的不同碰撞箱
        public GameObject ThisWeaponPrefab;
        #endregion

        #region 基础数值
        public float DamageValue_bas;//伤害
        public float _AttackRadius_bas;
        public float AttackRadius_bas{
            set{
                if(damageKind!=DamageKind.TrapWeapon&&Application.isPlaying){
                    _AttackRadius_bas = value;
                    if(damageKind==DamageKind.MeleeWeapon&&Range!=null){
                        Range.radius = value;
                    }
                }
                if(!Application.isPlaying){
                    _AttackRadius_bas = value;
                }
            }
            get { return _AttackRadius_bas;}
        }//攻击半径
        public float AttackInterval_bas;//攻击间隔
        public float MaxPower_bas;//满能量数值
        public float ExpulsionStrength;//击退力度

    #endregion

    public int Price { get {return this.value;} set{this.value=value;} }

    public void BeBought(Vector3 starrPos)
    {
        //TODO:处理动画效果
        GameObject PickWeapon=Instantiate(this.ThisWeaponPrefab,starrPos,Quaternion.identity);
        WeaponCtrl.Instance.ShowPickWeaponPanel(PickWeapon);
    }

    public void BeSoldOut()
    {
        //TODO:处理动画效果
    }

    private GoodType _goodtype = GoodType.Weapon;

    public GoodType GoodType
    {
        get => _goodtype; set => _goodtype = value;
    }
}
