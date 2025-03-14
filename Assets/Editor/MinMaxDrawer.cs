using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxValue))]
public class MinMaxDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // �������� ������������� �� ��� ����� - ��� min � max
        Rect minRect = new Rect(position.x, position.y, position.width / 2 - 3, position.height);
        Rect maxRect = new Rect(position.x + position.width / 2 + 3, position.y, position.width / 2 - 3, position.height);

        // ��������� �������� ��� min � max
        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("Min"), new GUIContent("Min"));
        EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("Max"), new GUIContent("Max"));
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // ������ ������ ����������
        return EditorGUIUtility.singleLineHeight;
    }
}
