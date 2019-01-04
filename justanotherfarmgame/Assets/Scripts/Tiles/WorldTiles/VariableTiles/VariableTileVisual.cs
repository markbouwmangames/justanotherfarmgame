using UnityEngine;

public class VariableTileVisual : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _baseSpriteRenderer;
    [SerializeField]
    private SpriteRenderer _topSpriteRenderer;

    private void Awake() {
        SetAlpha(ref _baseSpriteRenderer, 0f);
        SetAlpha(ref _topSpriteRenderer, 0f);
    }

    private void SetAlpha(ref SpriteRenderer renderer, float alpha) {
        var color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    public void SetBase(Sprite sprite) {
        _baseSpriteRenderer.sprite = sprite;
        SetAlpha(ref _baseSpriteRenderer, 1f);
    }

    public void SetTop(Sprite sprite) {
        _topSpriteRenderer.sprite = sprite;
        SetAlpha(ref _topSpriteRenderer, 1f);
    }
}
