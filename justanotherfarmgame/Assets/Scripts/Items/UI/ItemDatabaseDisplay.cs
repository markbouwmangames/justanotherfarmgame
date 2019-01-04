using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabaseDisplay : MonoBehaviour {
    [Header("References")]
    [SerializeField]
    private ItemSlotDisplay _itemSlotDisplay;
    [Header("Prefabs")]
    [SerializeField]
    private ItemCard _itemCardPrefab;

    private List<ItemSlot> _slots;

    private void Awake() {
        var database = GameManager.Instance.ItemDatabase;

        var items = database.GetAll();
        _slots = _itemSlotDisplay.AddSlots(items.Count);

        for (int i = 0; i < _slots.Count; i++) {
            var newItemInstance = new ItemInstance(items[i], 1);

            CreateItemCard(newItemInstance, _slots[i]);
            _slots[i].OnItemSlotUpdated += OnSlotUpdated;
        }
    }

    private void OnSlotUpdated(ItemSlot slot) {
        if (slot.ItemCard != null) return;

        var database = GameManager.Instance.ItemDatabase;
        var items = database.GetAll();

        var index = _slots.IndexOf(slot);
        var newItemInstance = new ItemInstance(items[index], 1);

        CreateItemCard(newItemInstance, slot);
    }

    private void CreateItemCard(ItemInstance item, ItemSlot slot) {
        var itemCard = GameObject.Instantiate(_itemCardPrefab);
        itemCard.Set(item);
        itemCard.SetToSlot(slot);
    }
}
