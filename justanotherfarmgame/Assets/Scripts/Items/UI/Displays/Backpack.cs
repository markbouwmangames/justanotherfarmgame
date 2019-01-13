using UnityEngine;

public class Backpack : InventoryDisplay {
    protected override void Update() {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (_isOpen == false) Open();
            else Close();
        }
    }
}
