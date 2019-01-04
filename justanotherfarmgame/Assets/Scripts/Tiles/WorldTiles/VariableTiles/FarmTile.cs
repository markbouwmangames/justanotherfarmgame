public class FarmTile : VariableTile {
    private FarmTileData _data;

    public FarmTile() {
        _data = GameManager.Instance.TileManager.FarmTileData;
    }

    public override void Initiate() {
        base.Initiate();

        _visual.SetBase(_data.BaseSprite);
    }

    public override void Plow() {
        if (!IsPlowed) {
            _visual.SetBase(_data.PlowedSprite_Dry);
        }

        base.Plow();
    }

    public override void Water() {
        if (!IsWatered && IsPlowed) {
            _visual.SetBase(_data.PlowedSprite_Watered);
            IsWatered = true;
        }
    }
}
