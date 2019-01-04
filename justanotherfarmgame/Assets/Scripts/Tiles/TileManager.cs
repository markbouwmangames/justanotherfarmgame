using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour {
    [Header("References")]
    [SerializeField]
    private Tilemap _tilemap;
    public VariableTileVisual VariableTileVisualPrefab;

    [Header("TileData")]
    public FarmTileData FarmTileData;

    [HideInInspector]
    public Dictionary<Vector3, WorldTile> Tiles;

    private void Awake() {
        if(_tilemap == null) {
            Debug.LogWarning("No tilemap set.");
            return;
        }
        GetWorldTiles();
    }

    private void GetWorldTiles() {
        Tiles = new Dictionary<Vector3, WorldTile>();

        foreach (Vector3Int pos in _tilemap.cellBounds.allPositionsWithin) {
            var localPos = new Vector3Int(pos.x, pos.y, pos.z);
            if (!_tilemap.HasTile(localPos)) continue;

            var tileBase = _tilemap.GetTile(localPos);

            var tile = InstantiateTile(tileBase);


            tile.LocalPos = localPos;
            tile.WorldPos = _tilemap.CellToWorld(localPos);
            tile.TileBase = tileBase;
            tile.Tilemap = _tilemap;

            tile.Initiate();

            if(tile.TileType != TileType.Background) {
                _tilemap.SetTileFlags(tile.LocalPos, TileFlags.None);
                _tilemap.SetColor(tile.LocalPos, new Color(0,0,0,0));
            }

            Tiles.Add(tile.WorldPos, tile);
        }
    }

    private WorldTile InstantiateTile(TileBase tileBase) {
        if (tileBase is CustomTileBase) {
            switch ((tileBase as CustomTileBase).TileType) {
                case TileType.Background:
                    return new WorldTile();
                case TileType.FarmTile:
                    return new FarmTile();
            }
        }

        return new WorldTile();
    }
}