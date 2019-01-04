using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Items/Item", fileName = "Item.asset")]
[System.Serializable]
public class Item : ScriptableObject {
    public string Id;
    public Sprite Icon;

    public int MaxStackSize;
}
