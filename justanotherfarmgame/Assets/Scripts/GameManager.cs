using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    [Header("Input")]
    public InputManager InputManager;

    [Header("Tiles")]
    public TileManager TileManager;

    [Header("Item Database")]
    public ItemDatabase ItemDatabase;
}
