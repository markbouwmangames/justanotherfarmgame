using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour {
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

            var tiles = GameManager.Instance.TileManager.Tiles; // This is our Dictionary of tiles
            WorldTile tile = null;

            if (tiles.TryGetValue(worldPoint, out tile)) {
                tile.Tilemap.SetTileFlags(tile.LocalPos, TileFlags.None);
                tile.Tilemap.SetColor(tile.LocalPos, Color.green);
            }
        }
    }
}
