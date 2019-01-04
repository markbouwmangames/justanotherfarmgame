using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FarmTileData))]
public class FarmTileDataEditor : Editor {
    public override void OnInspectorGUI() {
        var ftd = (FarmTileData) target;

        ftd.BaseSprite =            (Sprite)EditorGUILayout.ObjectField("Base", ftd.BaseSprite, typeof(Sprite), allowSceneObjects: false);
        ftd.PlowedSprite_Dry =      (Sprite)EditorGUILayout.ObjectField("Plowed-Dry", ftd.PlowedSprite_Dry, typeof(Sprite), allowSceneObjects: false);
        ftd.PlowedSprite_Watered =  (Sprite)EditorGUILayout.ObjectField("Plowed-Watered", ftd.PlowedSprite_Watered, typeof(Sprite), allowSceneObjects: false);
    }
}
