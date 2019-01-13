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
    public static ItemCard SelectedCard;

    private PlayerInputManager _inputManager;

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

        _inputManager = GameManager.Instance.PlayerInputManager;
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
    
    public void SetSelected() {
        SelectedCard = this;
    }

    private void Update() {
        if (SelectedCard != this && _isMoving == false) {
            transform.position = _previousSlot.transform.position;
        } else if (SelectedCard == this) {
            transform.SetAsLastSibling();
            transform.position = _inputManager.CursorPosition;
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
        if (SelectedCard == null) {
            if (eventData.button == PointerEventData.InputButton.Left || ItemInstance.Quantity == 1) {
                SelectedCard = this;
                _previousSlot.ItemCard = null;
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right) {
                ItemInstance.Quantity--;
                UpdateQuanitity();

                var newCard = GameObject.Instantiate(this);
                var newItem = new ItemInstance(ItemInstance.Item, 1);
                newCard.Set(newItem);

                SelectedCard = newCard;
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

                SelectedCard = newCard;
                newCard._previousSlot = _previousSlot;
                return;
            }
        }

        if (SelectedCard != this) {
            return;
        }

        var closestSlot = FindClosestSlot();
        if (closestSlot == null) {
            SetBack();
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left || ItemInstance.Quantity == 1) {
            if (closestSlot.ItemCard != null && closestSlot.ItemCard.ItemInstance.Id.Equals(ItemInstance.Id) == false) {
                var newCard = closestSlot.ItemCard;
                closestSlot.ItemCard = null;
                SetToSlot(closestSlot);
                SelectedCard = newCard;

                return;
            }

            if(closestSlot.ItemCard == null) {
                SetToSlot(closestSlot);
                SelectedCard = null;
                return;
            }

            var itemInstance = closestSlot.ItemCard.ItemInstance;
            var quantity = itemInstance.Quantity;
            var maxRemaining = itemInstance.Item.MaxStackSize - quantity;

            if (this.ItemInstance.Quantity <= maxRemaining) {
                SetToSlot(closestSlot);
                SelectedCard = null;
            } else {
                if(maxRemaining == 0) {
                    closestSlot.ItemCard.ItemInstance.Quantity = this.ItemInstance.Quantity;
                    closestSlot.ItemCard.UpdateQuanitity();

                    this.ItemInstance.Quantity = ItemInstance.Item.MaxStackSize;
                    UpdateQuanitity();
                } else {
                    ItemInstance.Quantity -= maxRemaining;
                    UpdateQuanitity();

                    var newCard = GameObject.Instantiate(this);
                    var newItem = new ItemInstance(ItemInstance.Item, maxRemaining);
                    newCard.Set(newItem);
                    newCard.SetToSlot(closestSlot);
                }
            }

            return;
        }

        if (eventData.button == PointerEventData.InputButton.Right) {
            if (closestSlot.ItemCard != null && closestSlot.ItemCard.ItemInstance.Id.Equals(ItemInstance.Id) == false) {
                return;
            }

            if(closestSlot.ItemCard != null) {
                var itemInstance = closestSlot.ItemCard.ItemInstance;
                var quantity = itemInstance.Quantity;
                if (quantity == itemInstance.Item.MaxStackSize) return;
            }

            ItemInstance.Quantity--;
            UpdateQuanitity();

            var newCard = GameObject.Instantiate(this);
            var newItem = new ItemInstance(ItemInstance.Item, 1);
            newCard.Set(newItem);
            newCard.SetToSlot(closestSlot);
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Middle) {
            if (closestSlot.ItemCard != null && closestSlot.ItemCard.ItemInstance.Id.Equals(ItemInstance.Id) == false) {
                return;
            }

            var halfToRemove = ItemInstance.Quantity / 2;

            if (closestSlot.ItemCard != null) {
                var itemInstance = closestSlot.ItemCard.ItemInstance;
                var quantity = itemInstance.Quantity;
                var maxRemaining = itemInstance.Item.MaxStackSize - quantity;

                if (halfToRemove > maxRemaining) {
                    halfToRemove = maxRemaining;
                }
            }

            ItemInstance.Quantity -= halfToRemove;
            UpdateQuanitity();

            var newCard = GameObject.Instantiate(this);
            var newItem = new ItemInstance(ItemInstance.Item, halfToRemove);
            newCard.Set(newItem);
            newCard.SetToSlot(closestSlot);
            return;
        }
    }

    public void SetBack() {
        if (_previousSlot == null) {
            Debug.LogError("No previous slot found for item.");
            return;
        }

        if (_previousSlot.ItemCard != null && _previousSlot.ItemCard.ItemInstance.Id.Equals(ItemInstance.Id) == false) {
            var parent = _previousSlot.ParentItemSlotDisplay;
            var firstOpenSlot = parent.GetFirstOpenSlot();
            SetToSlot(firstOpenSlot);
            AnimateTo(firstOpenSlot.transform.position);
            SelectedCard = null;
        } else {
            SetToSlot(_previousSlot);
            AnimateTo(_previousSlot.transform.position);
            SelectedCard = null;
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
