using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputManager : MonoBehaviour {
    [HideInInspector]
    public Vector3 CursorPosition;
    [HideInInspector]
    public float HorizontalMovement;
    [HideInInspector]
    public float VerticalMovement;
    [HideInInspector]
    public bool UsingController = true;

    [Header("References")]
    [SerializeField]
    private Image _cursorImage;

    private Vector3 _previousMousePos;

    private void Update() {
        UpdateMouseInput();
        UpdateKeyboardInput();
        UpdateControllerInput();
        UpdateCursorPos();
    }

    private void UpdateMouseInput() {
        var mousePos = Input.mousePosition;

        if(mousePos != _previousMousePos) {
            UsingController = false;
            _cursorImage.enabled = true;
        }

        if (UsingController) return;
        CursorPosition = _previousMousePos = mousePos;
    }

    private void UpdateKeyboardInput() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            VerticalMovement = 1;
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            VerticalMovement = -1;
        } else {
            VerticalMovement = 0;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            HorizontalMovement = -1;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            HorizontalMovement = 1;
        } else {
            HorizontalMovement = 0;
        }
    }

    private void UpdateControllerInput() {
        var horizontalValue = Input.GetAxis("Horizontal");
        if (horizontalValue < 0) HorizontalMovement = -1;
        else if (horizontalValue > 0) HorizontalMovement = 1;
        else HorizontalMovement = 0;

        var verticalValue = Input.GetAxis("Vertical");
        if (verticalValue < 0) VerticalMovement = -1;
        else if (verticalValue > 0) VerticalMovement = 1;
        else VerticalMovement = 0;

        if(horizontalValue != 0 || verticalValue != 0) {
            UsingController = true;
        }
    }

    private void UpdateCursorPos() {
        _cursorImage.rectTransform.position = CursorPosition;
    }
}
