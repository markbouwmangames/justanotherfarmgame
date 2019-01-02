using UnityEngine;

public class AutoHideCollisionMap : MonoBehaviour {
    [SerializeField]
    private bool _showInUnityEditor;

    private void Awake() {
#if UNITY_EDITOR
        if (_showInUnityEditor) return;
#endif

        var renderer = GetComponent<Renderer>();
        renderer.enabled = false;
    }
}
