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
            Collection_Data Data = Distributor.DistributeRandomCollectionbyLevel((int)Distributor.CollectionLevel);
            Distributor.DistributeCollection(Distributor.gameObject.transform.position,GameObject.FindGameObjectWithTag("Player").transform.position,Data);
            Debug.Log(Data.Name);
        }

        if (GUILayout.Button("Test_Dis_Prop_Random"))
        {
            Prop_Data Data = Distributor.DistributeRandomPropbyLevel((int)Distributor.PropLevel);
            Distributor.DistributeProp(Distributor.gameObject.transform.position,GameObject.FindGameObjectWithTag("Player").transform.position, Data);
            Debug.Log(Data?.Name);
        }

        if (GUILayout.Button("Test_WhenEnemyDies"))
        { 
            Distributor.WhenEnemyDies(Distributor.TestEnemy);
        }
    }
}
