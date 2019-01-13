using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using InControl;
using UnityEngine.EventSystems;

public class InventoryDisplay : MonoBehaviour {
    [Header("Settings")]
    [SerializeField]
    protected Inventory _inventory;
    [SerializeField]
    private float _controllerInputDelay;
    [SerializeField]
    private float _controllerTimeToHold;

    [Header("References")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _itemSlotParent;

    [Header("Prefabs")]
    [SerializeField]
    private ItemSlot _itemSlotPrefab;
    [SerializeField]
    private ItemCard _itemCardPrefab;

    protected List<ItemSlot> _itemSlots = new List<ItemSlot>();
    protected bool _isOpen;

    private float _currentHoldTime;
    private float _currentInputDelay;
    private bool _cursorIsOnSlots;
    private ItemSlot _currentSelected;
    private ItemCard _controllerSelectedItemCard;

    protected virtual void Awake() {
        Show(false);
    }

    protected virtual void Update() {
        if (_isOpen == false) return;
        HandleKeyboardInput();
    }

    private bool _isDown;

    private void HandleKeyboardInput() {
        var playerInputManager = GameManager.Instance.PlayerInputManager;
        if (playerInputManager.UsingController == false) {
            _currentHoldTime = 0f;
            _currentInputDelay = 0f;
            _cursorIsOnSlots = false;
            return;
        }

        CalculateMouseMovement(playerInputManager);

        var eventData = new PointerEventData(EventSystem.current);
        var activeDevice = InputManager.ActiveDevice;

        if (_isDown == false) {
            if (activeDevice.Action1 || activeDevice.Action2 || activeDevice.Action3 || activeDevice.Action4) {
                _isDown = true;
            }
        } else {
            if (!activeDevice.Action1 && !activeDevice.Action2 && !activeDevice.Action3 && !activeDevice.Action4) {
                _isDown = false;
            }

            return;
        }

        if (_controllerSelectedItemCard == null) {
            if (activeDevice.Action1 || activeDevice.Action3 || activeDevice.Action4) {
                _controllerSelectedItemCard = _currentSelected.ItemCard;
            }
        }

        if(_controllerSelectedItemCard != null) {
            if (activeDevice.Action2) {
                _controllerSelectedItemCard.SetBack();
                _controllerSelectedItemCard = null;
                return;
            }

            if (activeDevice.Action1) {
                eventData.button = PointerEventData.InputButton.Left;
                _controllerSelectedItemCard.OnPointerDown(eventData);
            }

            if (activeDevice.Action3) {
                eventData.button = PointerEventData.InputButton.Middle;
                _controllerSelectedItemCard.OnPointerDown(eventData);
            }

            if (activeDevice.Action4) {
                eventData.button = PointerEventData.InputButton.Right;
                _controllerSelectedItemCard.OnPointerDown(eventData);
            }
        }

        _controllerSelectedItemCard = ItemCard.SelectedCard;
    }

    private void CalculateMouseMovement(PlayerInputManager inputManager) {
        var xMovement = (int)inputManager.HorizontalMovement;
        var yMovement = (int)inputManager.VerticalMovement;

        if (xMovement == 0 && yMovement == 0) {
            _currentHoldTime = 0f;
            _currentInputDelay = 0f;
        }

        if (xMovement != 0 || yMovement != 0) {
            if (_currentHoldTime == 0) {
                MoveCursor(xMovement, yMovement);
            } else {
                if (_currentHoldTime > _controllerTimeToHold) {
                    _currentInputDelay -= Time.deltaTime;
                    if (_currentInputDelay < 0f) {
                        MoveCursor(xMovement, yMovement);
                        _currentInputDelay += _controllerInputDelay;
                    }
                }
            }

            _currentHoldTime += Time.deltaTime;
        }

        inputManager.CursorPosition = _currentSelected.transform.position;
    }

    private void MoveCursor(int xMovement, int yMovement) {
        if(_cursorIsOnSlots == false) {
            _currentSelected = _itemSlots[0];
            _cursorIsOnSlots = true;
            return;
        }

        var index = _itemSlots.IndexOf(_currentSelected);
        int row = Mathf.FloorToInt((float)index / 10f);
        int column = index % 10;

        var nextRow = Mathf.Clamp(row - yMovement, 0, Mathf.FloorToInt((float)_itemSlots.Count / 10f) - 1);
        var nextColumn = Mathf.Clamp(column + xMovement, 0, 9);
        var newIndex = (nextRow * 10) + nextColumn;

        _currentSelected = _itemSlots[newIndex];
    }

    public virtual void Open() {
        Show(true);
        UpdateDisplay();

        _currentSelected = _itemSlots[0];
    }

    public virtual void Close() {
        Show(false);
        UpdateInventory();

        foreach (var slot in _itemSlots) {
            if (slot.ItemCard != null) Destroy(slot.ItemCard.gameObject);
            Destroy(slot.gameObject);
        }

        _itemSlots.Clear();
    }

    private void Show(bool value) {
        _canvasGroup.alpha = value ? 1 : 0;
        _canvasGroup.interactable = value;
        _canvasGroup.blocksRaycasts = value;

        _isOpen = value;
    }

    private void UpdateDisplay() {
        for (var i = 0; i < _inventory.Items.Count; i++) {
            var itemSlot = AddSlot();
            var itemInstance = _inventory.Items[i];

            if (itemInstance.Item != null) {
                AddItemCard(itemInstance, itemSlot);
            }

            if (i == 0) _currentSelected = itemSlot;
        }
    }

    private void UpdateInventory() {
        for (var i = 0; i < _inventory.Items.Count; i++) {
            var itemSlot = _itemSlots[i];

            if (itemSlot.ItemCard == null) {
                _inventory.Items[i] = new ItemInstance(null, 0);
            } else {
                var itemInstance = itemSlot.ItemCard.ItemInstance;
                _inventory.Items[i] = new ItemInstance(itemInstance.Item, itemInstance.Quantity);
            }
        }
    }

    protected ItemSlot AddSlot() {
        var newSlot = GameObject.Instantiate(_itemSlotPrefab);
        newSlot.name = _itemSlotPrefab.name;
        newSlot.transform.SetParent(_itemSlotParent, true);
        newSlot.SetParentItemSlotDisplay(this);
        _itemSlots.Add(newSlot);
        return newSlot;
    }

    protected ItemCard AddItemCard(ItemInstance itemInstance, ItemSlot itemSlot) {
        var newItemCard = GameObject.Instantiate(_itemCardPrefab);
        newItemCard.Set(itemInstance);
        newItemCard.SetToSlot(itemSlot);
        return newItemCard;
    }

    public ItemSlot GetFirstOpenSlot() {
        return _itemSlots.FirstOrDefault(n => n.ItemCard == null);
    }
}
