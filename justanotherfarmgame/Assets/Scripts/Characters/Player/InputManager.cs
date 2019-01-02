using UnityEngine;

public class InputManager : MonoBehaviour {
    public float HorizontalMovement;
    public float VerticalMovement;

    private void Update() {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            VerticalMovement = 1;
        } else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
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
}
