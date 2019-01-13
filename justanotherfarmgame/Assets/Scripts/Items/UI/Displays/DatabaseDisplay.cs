using UnityEngine;

public class DatabaseDisplay : InventoryDisplay {
    private Item[] _items;

    protected override void Awake() {
        _items = Resources.LoadAll<Item>("Data/Items/");

        foreach(var item in _items) {
            _inventory.Add(new ItemInstance(item, 1));
        }

        Open();
    }

    protected override void Update() {
        //intentianally left blank
    }

    public override void Open() {
        base.Open();

        foreach(var slot in _itemSlots) {
            slot.OnItemSlotUpdated += OnItemSlotUpdated;
        }
    }

    private void OnItemSlotUpdated(ItemSlot itemSlot) {
        if (itemSlot.ItemCard != null) return;

        var index = _itemSlots.IndexOf(itemSlot);
        AddItemCard(new ItemInstance(_items[index], 1), itemSlot);
    }

    private void OnDestroy() {
        _inventory.Clear();
    }
}
