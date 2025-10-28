using DG.Tweening;
using UnityEngine;

public class CellView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;

    public void Initialize(Sprite sprite)
    {
        _renderer.sprite = sprite;
        Play();
    }

    private void Play()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.12f).SetEase(Ease.OutBounce);
    }
}
