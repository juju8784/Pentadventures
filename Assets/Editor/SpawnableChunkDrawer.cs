using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SpawnableChunk))]
public class SpawnableChunkDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Start the property so unity knows where we begin
        label = EditorGUI.BeginProperty(position, label, property);
        Rect contentPosition = EditorGUI.PrefixLabel(position, label);

        contentPosition.width *= 0.7f;     //It will now take up 60% of the width
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("chunk"), GUIContent.none);

        //Start the chance at the end of the other's width
        contentPosition.x += contentPosition.width + 2f;
        contentPosition.width = contentPosition.width * 3f / 7f;    
        contentPosition.width -= 2f;
        EditorGUIUtility.labelWidth = 20f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("weight"), new GUIContent("W"));
        EditorGUI.EndProperty();
    }
}
