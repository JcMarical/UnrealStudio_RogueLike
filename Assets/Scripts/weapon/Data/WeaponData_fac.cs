using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class WeaponData_fac
{
        #region 实际数值
        public float DamageValue_fac;//伤害
        public float AttachRadius_fac;//攻击半径
        public float AttachInterval_fac;//攻击间隔
        public float ExpulsionStrength_fac;//击退能力
        public float DefaultCharge_Value;//默认单次攻击充能量
        public float Weight_fac;//武器重量
       #endregion
        public WeaponData_fac(WeaponData aa){
            DamageValue_fac = aa.DamageValue_bas;
            AttachRadius_fac = aa.AttachRadius_bas;
            AttachInterval_fac=aa.AttachInterval_bas;
            ExpulsionStrength_fac = aa.ExpulsionStrength;
            Weight_fac=aa.Weight_bas;
            DefaultCharge_Value = aa.DefaultCharge;
        }
        public void UpdateData(WeaponData aa){
            DamageValue_fac = aa.DamageValue_bas;
            AttachRadius_fac = aa.AttachRadius_bas;
            AttachInterval_fac=aa.AttachInterval_bas;
            ExpulsionStrength_fac = aa.ExpulsionStrength;
            Weight_fac=aa.Weight_bas;
            DefaultCharge_Value = aa.DefaultCharge;
        }
}
