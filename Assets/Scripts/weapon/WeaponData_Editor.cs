using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.Rendering;

[CustomEditor(typeof(WeaponData))]
public class WeaponData_Editor : Editor {
    private WeaponData weaponData;
    private SerializedObject serializedObject;
    private void OnEnable() {
        weaponData=(WeaponData) target;
        serializedObject =new SerializedObject(weaponData);
    }
    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rarity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damageKind"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Weight_bas"));
        switch(weaponData.damageKind){
            case DamageKind.MeleeWeapon:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("segment"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("specialEffect"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Range"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DamageValue_bas"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AttackRadius_bas"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxPower_bas"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AttackInterval_bas"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ExpulsionStrength"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultCharge"));
                break;
            case DamageKind.RangedWeapon:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("specialEffect"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DamageValue_bas"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AttackRadius_bas"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxPower_bas"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AttackInterval_bas"));       
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ExpulsionStrength"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DefaultCharge"));
                break;
            case DamageKind.TrapWeapon:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("AttackInterval_bas"));
                break;
        }
        serializedObject.ApplyModifiedProperties();  
    }
}
