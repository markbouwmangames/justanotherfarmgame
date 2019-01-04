using System.Collections.Generic;
using UnityEngine;

public class ItemSlotDisplay : MonoBehaviour {
    [Header("Settings")]
    [SerializeField]
    private int _numSlots;
    [Header("References")]
    [SerializeField]
    private RectTransform _itemSlotParent;
    [Header("Prefabs")]
    [SerializeField]
    private ItemSlot _itemSlotPrefab;

    [HideInInspector]
    public List<ItemSlot> ItemSlots = new List<ItemSlot>();

    private void Awake() {
        AddSlots(_numSlots);
    }

    public List<ItemSlot> AddSlots(int num) {
        var newItemSlots = new List<ItemSlot>();

        for (int i = 0; i < num; i++) {
            var newSlot = GameObject.Instantiate(_itemSlotPrefab);
            newSlot.name = _itemSlotPrefab.name;
            newSlot.transform.SetParent(_itemSlotParent, true);

            newItemSlots.Add(newSlot);
        }

        ItemSlots.AddRange(newItemSlots);

        return newItemSlots;
    }
}
