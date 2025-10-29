using DG.Tweening;
using UnityEngine;

public class CellView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    public void Initialize(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }
}
