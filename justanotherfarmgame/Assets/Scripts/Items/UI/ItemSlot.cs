using System;
using UnityEngine;

public class ItemSlot : MonoBehaviour {
    public Action<ItemSlot> OnItemSlotUpdated;

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
}
