using DG.Tweening;
using UnityEngine;
using Zenject;

public class Popup : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float appearScaleDuration = 0.2f; // время анимации появления скейлом
    [SerializeField] private float delayBeforeFly = 0.15f; // пауза перед полётом
    [SerializeField] private float flyDuration = 0.6f;  // длительность полёта

    [Header("Path / Curve")]
    [SerializeField] private float arcHeight = 2f;    // высота дуги (чем больше, тем выше траектория)
    [SerializeField] private bool destroyOnArrive = true;  // уничтожить объект по прилёту

    private RectTransform targetUI;

    private Sequence _seq;

    public void Initialize(RectTransform target)
    {
        targetUI = target;
        Play();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }

    public void Play()
    {
        _seq?.Kill();

        Vector3 startPos = transform.position;
        Vector3 endPos = targetUI.position;
        endPos.z = startPos.z;

        Vector3 midPos = (startPos + endPos) * 0.5f;
        midPos += Vector3.up * arcHeight;

        Vector3[] path = new Vector3[] { startPos, midPos, endPos };

        _seq = DOTween.Sequence();

        _seq.Append(
            transform
                .DOScale(1f, appearScaleDuration)
                .SetEase(Ease.OutBack)
        );

        _seq.AppendInterval(delayBeforeFly);

        _seq.Append(
            transform
                .DOPath(
                    path,
                    flyDuration,
                    PathType.CatmullRom,
                    PathMode.Ignore
                )
                .SetEase(Ease.OutCubic)
        );

        _seq.OnComplete(() =>
        {
            if (destroyOnArrive)
            {
                Destroy(gameObject);
            }
        });
    }

    private void OnDisable()
    {
        _seq?.Kill();
    }


    public class Factory : PlaceholderFactory<Popup>
    {
    }
}
