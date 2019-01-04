using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/Tiles/CustomTileBase", fileName = "CustomTileBase.asset")]
public class CustomTileBase : Tile {
    public TileType TileType;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        if (Application.isPlaying) {
            tileData.sprite = null;
        }

        base.GetTileData(position, tilemap, ref tileData); 
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
        return base.StartUp(position, tilemap, go);
    }
}
