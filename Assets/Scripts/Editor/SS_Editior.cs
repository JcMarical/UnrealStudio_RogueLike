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
            foreach (GameObject Target in SSMgr.Targets)
            {
                SSMgr.AddSpecialState(Target,SSMgr.State, SSMgr.Duration);
            }
        }

        if (GUILayout.Button("RemoveState"))
        {   
            foreach (GameObject Target in SSMgr.Targets)
            {
                SSMgr.RemoveSpecialState(Target, (SpecialState)CreateInstance(SSMgr.State.GetType()));
            }
        }

        if (GUILayout.Button("RemoveAllState"))
        {
            foreach (GameObject Target in SSMgr.Targets)
            {
                SSMgr.RemoveAllSpecialState(Target);
            }
        }
    }
}
