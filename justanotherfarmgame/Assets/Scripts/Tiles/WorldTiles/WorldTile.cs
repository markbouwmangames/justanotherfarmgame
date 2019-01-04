using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile {
    public Vector3Int LocalPos;
    public Vector3 WorldPos;
    public TileBase TileBase;
    public Tilemap Tilemap;

    public bool IsPlowed;
    public bool IsWatered;

    public TileType TileType {
        get {
            if ((TileBase is CustomTileBase) == false) return TileType.Background;
            return (TileBase as CustomTileBase).TileType;
        }
    }

    public virtual void Initiate() {

    }

    public virtual void Plow() {
        if(IsPlowed == false) {
            IsPlowed = true;
        }
    }

    public virtual void Water() {
        if(IsWatered == false) {
            IsWatered = true;
        }
    }
}