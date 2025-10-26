using DG.Tweening;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private RectTransform target;
    // Если оставить null, возьмём RectTransform с этого объекта

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 40f;      // Насколько высоко подпрыгиваем (в пикселях UI)
    [SerializeField] private float jumpDuration = 0.6f;   // Общая длительность одного прыжка (сек)

    [Header("Tilt (Z-rotation)")]
    [SerializeField] private float tiltAngle = 8f;        // Максимальный наклон по Z (в градусах)
    [SerializeField] private float tiltDuration = 0.4f;   // Время раскачки в одну сторону

    // Внутренние твины (чтобы корректно убивать при Disable)
    private Sequence jumpSequence;
    private Tween tiltTween;

    // Запоминаем стартовую позицию, чтобы всегда возвращаться точно туда
    private Vector2 startAnchoredPos;
    private Vector3 startEulerAngles;

    private void Awake()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        startAnchoredPos = target.anchoredPosition;
        startEulerAngles = target.localEulerAngles;
    }

    private void OnEnable()
    {
        PlayJumpLoop();
        PlayTiltLoop();
    }

    private void OnDisable()
    {
        // Очень важно: чистим твины, иначе DOTween будет пытаться анимировать уже выключенный объект
        if (jumpSequence != null && jumpSequence.IsActive()) jumpSequence.Kill();
        if (tiltTween != null && tiltTween.IsActive()) tiltTween.Kill();

        // Возвращаем объект в исходное состояние
        target.anchoredPosition = startAnchoredPos;
        target.localEulerAngles = startEulerAngles;
    }

    private void PlayJumpLoop()
    {
        // Создаём новую последовательность прыжка
        jumpSequence = DOTween.Sequence();

        // Фаза вверх: быстро выстреливаем вверх
        jumpSequence.Append(
            target.DOAnchorPosY(startAnchoredPos.y + jumpHeight, jumpDuration * 0.4f)
                  .SetEase(Ease.OutQuad) // быстро стартует и замедляется к пику
        );

        // Фаза вниз: падаем обратно с отскоком (bounce)
        jumpSequence.Append(
            target.DOAnchorPosY(startAnchoredPos.y, jumpDuration * 1)
                  .SetEase(Ease.OutBounce) // тот самый "бууууунс"
        );

        // Делаем бесконечный цикл
        jumpSequence.SetLoops(-1, LoopType.Restart);

        // Опционально: чтобы анимация продолжалась даже при паузе игры (UI обычно так хотят)
        // jumpSequence.SetUpdate(true);
    }

    private void PlayTiltLoop()
    {
        // Качаем объект туда-сюда по оси Z
        // Yoyo = вперёд-назад-вперёд...
        tiltTween = target.DOLocalRotate(
                            new Vector3(
                                startEulerAngles.x,
                                startEulerAngles.y,
                                startEulerAngles.z + tiltAngle
                            ),
                            tiltDuration,
                            RotateMode.Fast
                        )
                        .SetEase(Ease.InOutSine)
                        .SetLoops(-1, LoopType.Yoyo);

        // Если тоже хотим, чтобы это работало в паузе — раскомментируй:
        // tiltTween.SetUpdate(true);
    }
}
