using System;
using UnityEngine;

public class ItemSlot : MonoBehaviour {
    public Action<ItemSlot> OnItemSlotUpdated;
    public InventoryDisplay ParentItemSlotDisplay;

    public ItemCard ItemCard {
        get {
            return _currentItemCard;
        }
        set {
            _currentItemCard = value;
            if (OnItemSlotUpdated != null) OnItemSlotUpdated.Invoke(this);
        }
    }

    private ItemCard _currentItemCard;

    public void SetParentItemSlotDisplay(InventoryDisplay itemSlotDisplay) {
        ParentItemSlotDisplay = itemSlotDisplay;
    }
}
