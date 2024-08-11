using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropDistributor))]
public class Distribute_Edi : Editor
{
    private PropDistributor Distributor = PropDistributor.Instance;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Test_Dis_Collection_Random"))
        {
            Debug.Log(Distributor.DistributeRandomCollectionbyLevel(Distributor.CollectionLevel)?.Name);
        }

        if (GUILayout.Button("Test_Dis_Prop_Random"))
        { 
            Debug.Log(Distributor.DistributeRandomPropbyLevel(Distributor.PropLevel)?.Name);
        }

        if (GUILayout.Button("Test_WhenEnemyDies"))
        { 
            Distributor.WhenEnemyDies(Distributor.TestEnemy);
        }
    }
}
