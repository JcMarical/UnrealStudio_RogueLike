using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ObtainableObjectData))]
public class PropDataEditor : Editor
{
    private SerializedProperty propIconProperty;

    private void OnEnable()
    {
        // 在OnEnable中获取SerializedProperty引用
        propIconProperty = serializedObject.FindProperty("Icon");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update(); // 开始处理SerializedObject

        EditorGUILayout.BeginVertical();
        EditorGUILayout.PrefixLabel("\n");
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal(); // 开始水平布局
        EditorGUILayout.PrefixLabel("Icon");

        // 绘制Sprite预览框
        GUIContent label = new GUIContent("道具图标", "拖拽一个Sprite资源到这里");
        Rect spriteFieldRect = GUILayoutUtility.GetRect(64, 64, GUILayout.ExpandWidth(false)); // 设置预览框大小
        EditorGUI.DrawRect(spriteFieldRect, Color.clear); // 绘制透明背景

        // 绘制Sprite字段
        EditorGUI.BeginChangeCheck(); // 开始检查是否有更改
        propIconProperty.objectReferenceValue = EditorGUI.ObjectField(spriteFieldRect, propIconProperty.objectReferenceValue, typeof(Sprite), false);
        bool wasChanged = EditorGUI.EndChangeCheck(); // 结束检查更改

        if (wasChanged && propIconProperty.objectReferenceValue != null)
        {
            // 如果有更改并且用户选择了一个Sprite，则更新预览
            Sprite sprite = propIconProperty.objectReferenceValue as Sprite;
            if (sprite != null)
            {
                // 根据Sprite的尺寸调整预览框的宽高比
                float aspectRatio = (float)sprite.rect.width / sprite.rect.height;
                if (aspectRatio > 1f)
                {
                    spriteFieldRect.width = spriteFieldRect.height * aspectRatio;
                }
                else
                {
                    spriteFieldRect.height = spriteFieldRect.width / aspectRatio;
                }
            }
        }
        EditorGUILayout.EndHorizontal(); // 结束水平布局

        serializedObject.ApplyModifiedProperties(); // 应用SerializedObject的更改
    }
}