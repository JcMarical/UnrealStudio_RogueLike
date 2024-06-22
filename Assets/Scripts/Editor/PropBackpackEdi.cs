using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropBackPackUIMgr))]
public class PropBackpackEdi : Editor
{
    [SerializeField]public PropData testData_Edi;
    private PropBackPackUIMgr myMgr = PropBackPackUIMgr.Instance;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 保留原有的Inspector

        testData_Edi = (PropData)EditorGUILayout.ObjectField("Test Data Edi", testData_Edi, typeof(PropData), false);

        if (GUILayout.Button("AddProp"))
        {
            myMgr.AddProp(testData_Edi);
        }
        if (GUILayout.Button("ShowPropBackpack"))
        {
            myMgr.ShowPropBackpack();
        }
        if (GUILayout.Button("HidePropBackpack"))
        {
            myMgr.HidePropBackpack();
        }
    }
}
