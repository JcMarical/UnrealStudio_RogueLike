using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(StoreRoomMgr))]
public class StoreRoomEditor : OdinEditor
{
    StoreRoomMgr Mgr;

    private void Awake()
    {
        Mgr = StoreRoomMgr.Instance;
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new();
        IMGUIContainer container = new IMGUIContainer(() => base.DrawDefaultInspector());
        root.Add(container);
        return root;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Storage"))
        {
            Mgr.Storage(Mgr.StoreTestAmount);
        }

        if (GUILayout.Button("TakeOut"))
        {
            Mgr.TakeOut(Mgr.TakeOutTestAmount);
        }
    }
}
