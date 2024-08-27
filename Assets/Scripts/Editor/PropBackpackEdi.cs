using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropBackPackUIMgr))]
public class PropBackpackEdi : Editor
{
    [SerializeField]public Prop_Data testData_Edi;
    private PropBackPackUIMgr myMgr = PropBackPackUIMgr.Instance;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 保留原有的Inspector

        testData_Edi = (Prop_Data)EditorGUILayout.ObjectField("Test Data Edi", testData_Edi, typeof(Prop_Data), false);

        if (GUILayout.Button("ShowPropBackpack"))
        {
            myMgr.ShowPropBackpack();
        }
        if (GUILayout.Button("HidePropBackpack"))
        {
            myMgr.HidePropBackpack();
        }
        if (GUILayout.Button("AddMoney"))
        {
            myMgr.GainCoin(100);
        }
        if (GUILayout.Button("AddProp"))
        {
            myMgr.GetProp(testData_Edi);
        }
        if (GUILayout.Button("UseProp"))
        {
            myMgr.UseProp();
        }
        if (GUILayout.Button("SwitchProp"))
        {
            myMgr.SwitchPropsList();
        }
        
    }
}
