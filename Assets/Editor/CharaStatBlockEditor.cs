using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharaStatBlock))]
public class CharaStatBlockEditor : Editor
{
    public override void OnInspectorGUI()
    {

        CharaStatBlock chara = (CharaStatBlock)target;

        GUILayout.Label("General Information and Stats\n ");
        base.OnInspectorGUI();
        GUILayout.Label("Inventory ^ ");
        EditorGUILayout.HelpBox(" 0 is no item.\n 1 is common.\n 2 is uncommon.\n 3 is rare.\n 4 is legendary.\n 5 is artifac.", MessageType.None);

    }
}
