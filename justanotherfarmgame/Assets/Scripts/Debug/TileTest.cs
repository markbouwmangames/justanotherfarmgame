using UnityEngine;

public class TileTest : MonoBehaviour {
    private void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

            var tiles = GameManager.Instance.TileManager.Tiles; // This is our Dictionary of tiles
            WorldTile tile = null;

            if (tiles.TryGetValue(worldPoint, out tile)) {

                if(Input.GetMouseButtonDown(0)) {
                    tile.Plow();
                } 
                if(Input.GetMouseButtonDown(1)) {
                    tile.Water();
                }
            }
        }
    }
}
