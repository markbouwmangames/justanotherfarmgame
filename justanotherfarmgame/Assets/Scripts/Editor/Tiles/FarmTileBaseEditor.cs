using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomTileBase))]
public class FarmTileBaseEditor : Editor {
    public override void OnInspectorGUI() {
        var ctb = (CustomTileBase)target;

        EditorGUILayout.LabelField("Sprite");
        ctb.sprite = (Sprite)EditorGUILayout.ObjectField("", ctb.sprite, typeof(Sprite), allowSceneObjects: false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Color");
        ctb.color = EditorGUILayout.ColorField(ctb.color);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Flags");
        EditorGUILayout.EnumPopup(ctb.flags);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Collider Type");
        EditorGUILayout.EnumPopup(ctb.colliderType);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tile Type");
        EditorGUILayout.EnumPopup(ctb.TileType);
    }
}
