using UnityEngine;

public class Player : Character {
    protected override void Update() {
        GetInput();
        base.Update();
    }

    private void GetInput() {
        var inputManager = GameManager.Instance.InputManager;

        var horizontal = inputManager.HorizontalMovement;
        var vertical = inputManager.VerticalMovement;

        direction.x = horizontal == 0 ? 0 : Mathf.Sign(horizontal);
        direction.y = vertical == 0 ? 0 : Mathf.Sign(vertical);
    }
}
