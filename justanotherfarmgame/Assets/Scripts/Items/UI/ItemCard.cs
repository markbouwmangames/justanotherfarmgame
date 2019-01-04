using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private Text _quantityTextfield;

    public ItemInstance ItemInstance;

    private bool _isMoving;
    private ItemSlot _previousSlot;

    private static RectTransform _itemCardCanvas;
    private static ItemCard _selectedCard;

    private void Awake() {
        if(_itemCardCanvas == null) {
            var itemCardCanvasGO = GameObject.Find("ItemCardCanvas");
            if(itemCardCanvasGO == null) {
                Debug.LogError("Could not find an ItemCardCanvas to hold ItemCards");
                return;
            }
            _itemCardCanvas = itemCardCanvasGO.GetComponent<RectTransform>();
        }

        _quantityTextfield.text = "";

        transform.SetParent(_itemCardCanvas, true);
    }

    public void Set(ItemInstance itemInstance) {
        ItemInstance = itemInstance;
        _icon.sprite = itemInstance.Item.Icon;
        _icon.SetNativeSize();
        UpdateQuanitity();
    }

    public void UpdateQuanitity() {
        _quantityTextfield.text = ItemInstance.Quantity == 1 ? "" : ItemInstance.Quantity.ToString();
    }
    
    private void Update() {
        if (_selectedCard != this && _isMoving == false) {
            transform.position = _previousSlot.transform.position;
        } else if (_selectedCard == this) {
            transform.SetAsLastSibling();
            transform.position = (Vector3)(Input.mousePosition);
        }
    }

    private ItemSlot FindClosestSlot() {
        var slots = FindObjectsOfType<ItemSlot>();
        ItemSlot closestSlot = null;
        float distance = float.MaxValue;
        foreach (var slot in slots) {
            var d = Vector3.Distance(slot.transform.position, transform.transform.position);
            if (d < distance) {
                closestSlot = slot;
                distance = d;
            }
        }
        if (distance < 150) {
            return closestSlot;
        } else {
           return null;
        }
    }

    public void SetToSlot(ItemSlot slot) {
        var updateQuantity = slot.ItemCard != null;

        if (updateQuantity == false) {
            _previousSlot = slot;
            _previousSlot.ItemCard = this;
        } else {
            slot.ItemCard.ItemInstance.Quantity += ItemInstance.Quantity;
            slot.ItemCard.UpdateQuanitity();
            Destroy(gameObject);
        }
    }

    private void AnimateTo(Vector3 target, Action onComplete = null) {
        var distance = Vector3.Distance(transform.position, target);

        var farTime = 0.25f;
        var closeTime = 0.1f;
        var p = (1 - Mathf.Clamp01(distance / 1250f));
        var time = Mathf.Lerp(farTime, closeTime, p);

        _isMoving = true;

        transform.DOMove(target, time).OnComplete(() => {
            _isMoving = false;
            if (onComplete != null) onComplete();
        });
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (_selectedCard == null) {
            if (eventData.button == PointerEventData.InputButton.Left || ItemInstance.Quantity == 1) {
                _selectedCard = this;
                _previousSlot.ItemCard = null;
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right) {
                ItemInstance.Quantity--;
                UpdateQuanitity();

                var newCard = GameObject.Instantiate(this);
                var newItem = new ItemInstance(ItemInstance.Item, 1);
                newCard.Set(newItem);

                _selectedCard = newCard;
                newCard._previousSlot = _previousSlot;
                return;
            }

            if(eventData.button == PointerEventData.InputButton.Middle) {
                var half = ItemInstance.Quantity / 2;
                ItemInstance.Quantity -= half;
                UpdateQuanitity();

                var newCard = GameObject.Instantiate(this);
                var newItem = new ItemInstance(ItemInstance.Item, half);
                newCard.Set(newItem);

                _selectedCard = newCard;
                newCard._previousSlot = _previousSlot;
                return;
            }
        }

        if (_selectedCard != this) {
            return;
        }

        var closestSlot = FindClosestSlot();
        if (closestSlot == null) {
            if(_previousSlot == null) {
                return;
            }

            SetToSlot(_previousSlot);
            AnimateTo(_previousSlot.transform.position);
            _selectedCard = null;
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left || ItemInstance.Quantity == 1) {
            SetToSlot(closestSlot);
            _selectedCard = null;
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            ItemInstance.Quantity--;
            UpdateQuanitity();

            var newCard = GameObject.Instantiate(this);
            var newItem = new ItemInstance(ItemInstance.Item, 1);
            newCard.Set(newItem);
            newCard.SetToSlot(closestSlot);
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Middle) {
            var half = ItemInstance.Quantity / 2;
            ItemInstance.Quantity -= half;
            UpdateQuanitity();

            var newCard = GameObject.Instantiate(this);
            var newItem = new ItemInstance(ItemInstance.Item, half);
            newCard.Set(newItem);
            newCard.SetToSlot(closestSlot);
            return;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
    }

    public void OnBeginDrag(PointerEventData eventData) {
    }

    public void OnDrag(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {
    }
}
