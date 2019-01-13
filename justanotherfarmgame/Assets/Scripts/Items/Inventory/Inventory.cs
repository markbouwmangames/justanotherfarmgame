using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Custom/Items/Inventory", fileName = "Inventory.asset")]
[System.Serializable]
public class Inventory : ScriptableObject {
    public List<ItemInstance> Items;

    public Inventory() {
        Items = new List<ItemInstance>();
    }

    public void Add(params ItemInstance[] items) {
        foreach (var item in items) {
            Items.Add(item);
        }
    }

    public void Remove(params ItemInstance[] items) {
        foreach (var item in items) {
            if (Items.Contains(item)) {
                Items.Remove(item);
            }
        }
    }

    public void Clear() {
        Items = new List<ItemInstance>();
    }
}
