using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile {
    public Vector3Int LocalPos { get; set; }

    public Vector3 WorldPos { get; set; }

    public TileBase TileBase { get; set; }

    public Tilemap Tilemap { get; set; }
}