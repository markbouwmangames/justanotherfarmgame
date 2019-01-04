using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor {
    public override void OnInspectorGUI() {
        var item = (Item)target;

        EditorGUILayout.HelpBox("Item Data", MessageType.None);
        item.Id = EditorGUILayout.TextField("Id", item.Id);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Item Variables", MessageType.None);

        item.MaxStackSize = EditorGUILayout.IntField("Max Stack Size", item.MaxStackSize);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Visuals", MessageType.None);

        EditorGUILayout.LabelField("Sprite");
        item.Icon = (Sprite)EditorGUILayout.ObjectField("", item.Icon, typeof(Sprite), allowSceneObjects: false);
    }
}
