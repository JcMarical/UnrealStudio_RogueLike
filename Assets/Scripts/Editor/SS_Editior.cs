using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SS_Mgr))]
public class SS_Editior : Editor
{

    private SS_Mgr SSMgr;
    private void OnEnable()
    {
        SSMgr = SS_Mgr.Instance;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("AddSpecialState"))
        {
            GameObject target = GameObject.Find("Player");
            SSMgr.AddSpecialState(target, SSMgr.State, SSMgr.Duration,SSMgr.From);
        }

        if (GUILayout.Button("RemoveState"))
        {   
            foreach (var Target in SSMgr.Targets)
            {
                SSMgr.RemoveSpecialState(Target.gameObject, (SpecialState)CreateInstance(SSMgr.State.GetType()));
            }
        }

        if (GUILayout.Button("RemoveAllState"))
        {
            foreach (var Target in SSMgr.Targets)
            {
                SSMgr.RemoveAllSpecialState(Target.gameObject);
            }
        }
    }
}
