using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.Rendering;

[CustomEditor(typeof(WeaponData))]
public class WeaponData_Editor : Editor {
    private WeaponData weaponData;
    private SerializedObject aa;
    private void OnEnable() {
        weaponData=(WeaponData) target;
        aa =new SerializedObject(weaponData);
    }
    public override void OnInspectorGUI() {
        aa.Update();
        EditorGUILayout.PropertyField(aa.FindProperty("rarity"));
        EditorGUILayout.PropertyField(aa.FindProperty("id"));
        EditorGUILayout.PropertyField(aa.FindProperty("value"));
        EditorGUILayout.PropertyField(aa.FindProperty("sprite"));                
        EditorGUILayout.PropertyField(aa.FindProperty("specialEffect"));
        EditorGUILayout.PropertyField(aa.FindProperty("damageKind"));
        switch(weaponData.damageKind){
            case DamageKind.MeleeWeapon:
                EditorGUILayout.PropertyField(aa.FindProperty("Range"));
                EditorGUILayout.PropertyField(aa.FindProperty("DamageValue_bas"));
                EditorGUILayout.PropertyField(aa.FindProperty("_AttackRadius_bas"));
                EditorGUILayout.PropertyField(aa.FindProperty("MaxPower_bas"));
                EditorGUILayout.PropertyField(aa.FindProperty("AttackInterval_bas"));
                EditorGUILayout.PropertyField(aa.FindProperty("ExpulsionStrength"));
                break;
            case DamageKind.RangedWeapon:
                EditorGUILayout.PropertyField(aa.FindProperty("DamageValue_bas"));
                EditorGUILayout.PropertyField(aa.FindProperty("_AttackRadius_bas"));
                EditorGUILayout.PropertyField(aa.FindProperty("MaxPower_bas"));
                EditorGUILayout.PropertyField(aa.FindProperty("AttackInterval_bas"));       
                EditorGUILayout.PropertyField(aa.FindProperty("ExpulsionStrength"));
                break;
            case DamageKind.TrapWeapon:
                EditorGUILayout.PropertyField(aa.FindProperty("AttackInterval_bas"));
                break;
        }
        aa.ApplyModifiedProperties();  
    }
}
