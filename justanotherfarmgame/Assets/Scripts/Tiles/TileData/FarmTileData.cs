using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Tiles/FarmTileData", fileName = "FarmTileData.asset")]
public class FarmTileData : ScriptableObject {
    public Sprite BaseSprite;
    public Sprite PlowedSprite_Dry;
    public Sprite PlowedSprite_Watered;
}
