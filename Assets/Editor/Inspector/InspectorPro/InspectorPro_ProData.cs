using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PropData))]
public class PropDataEditor : Editor
{
    private SerializedProperty propIconProperty;

    private void OnEnable()
    {
        // ��OnEnable�л�ȡSerializedProperty����
        propIconProperty = serializedObject.FindProperty("PropIcon");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update(); // ��ʼ����SerializedObject

        EditorGUILayout.BeginVertical();
        EditorGUILayout.PrefixLabel("\n");
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal(); // ��ʼˮƽ����
        EditorGUILayout.PrefixLabel("PropIcon");

        // ����SpriteԤ����
        GUIContent label = new GUIContent("����ͼ��", "��קһ��Sprite��Դ������");
        Rect spriteFieldRect = GUILayoutUtility.GetRect(64, 64, GUILayout.ExpandWidth(false)); // ����Ԥ�����С
        EditorGUI.DrawRect(spriteFieldRect, Color.clear); // ����͸������

        // ����Sprite�ֶ�
        EditorGUI.BeginChangeCheck(); // ��ʼ����Ƿ��и���
        propIconProperty.objectReferenceValue = EditorGUI.ObjectField(spriteFieldRect, propIconProperty.objectReferenceValue, typeof(Sprite), false);
        bool wasChanged = EditorGUI.EndChangeCheck(); // ����������

        if (wasChanged && propIconProperty.objectReferenceValue != null)
        {
            // ����и��Ĳ����û�ѡ����һ��Sprite�������Ԥ��
            Sprite sprite = propIconProperty.objectReferenceValue as Sprite;
            if (sprite != null)
            {
                // ����Sprite�ĳߴ����Ԥ����Ŀ�߱�
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
        EditorGUILayout.EndHorizontal(); // ����ˮƽ����

        serializedObject.ApplyModifiedProperties(); // Ӧ��SerializedObject�ĸ���
    }
}