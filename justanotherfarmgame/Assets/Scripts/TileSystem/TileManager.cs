using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour {
    [SerializeField]
    private Tilemap _tilemap;

    [HideInInspector]
    public Dictionary<Vector3, WorldTile> Tiles;

    private void Awake() {
        GetWorldTiles();
    }

    private void GetWorldTiles() {
        Tiles = new Dictionary<Vector3, WorldTile>();

        

        foreach (Vector3Int pos in _tilemap.cellBounds.allPositionsWithin) {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);

            if (!_tilemap.HasTile(localPos)) continue;
            var tile = new WorldTile {
                LocalPos = localPos,
                WorldPos = _tilemap.CellToWorld(localPos),
                TileBase = _tilemap.GetTile(localPos),
                Tilemap = _tilemap,
            };

            Tiles.Add(tile.WorldPos, tile);
        }
    }
}