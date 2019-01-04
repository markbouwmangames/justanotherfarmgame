using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {
    private List<Item> _items;

    private void Awake() {
        _items = Resources.LoadAll<Item>("Data/Items").ToList();
    }

    public Item Get(string id) {
        return _items.FirstOrDefault(n => n.Id.Equals(id));
    }

    public List<Item> GetAll() {
        return _items;
    }
}
