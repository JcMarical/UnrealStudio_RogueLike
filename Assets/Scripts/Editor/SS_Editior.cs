using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SS_Mgr))]
public class SS_Editior : Editor
{

    private SS_Mgr SSMgr = SS_Mgr.Instance;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("AddSpecialState"))
        { 
            SSMgr.AddSpecialState(SSMgr.Target, SSMgr.State, SSMgr.Duration);
        }

        if (GUILayout.Button("RemoveState"))
        { 
            SSMgr.RemoveSpecialState(SSMgr.Target, SSMgr.State);
        }

        if (GUILayout.Button("RemoveAllState"))
        {
            SSMgr.RemoveAllSpecialState(SSMgr.Target);
        }
    }
}
