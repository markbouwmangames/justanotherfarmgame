using UnityEngine;

public abstract class VariableTile : WorldTile {
    //constants
    private Vector3 TileOffset = new Vector3(0.5f, 0.5f, 0);

    //variables
    protected VariableTileVisual _visual;

    public override void Initiate() {
        base.Initiate();

        var prefab = GameManager.Instance.TileManager.VariableTileVisualPrefab;
        _visual = GameObject.Instantiate(prefab);
        _visual.transform.position = WorldPos + TileOffset;
    }

}
