using DG.Tweening;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private RectTransform target;
    // ���� �������� null, ������ RectTransform � ����� �������

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 40f;      // ��������� ������ ������������ (� �������� UI)
    [SerializeField] private float jumpDuration = 0.6f;   // ����� ������������ ������ ������ (���)

    [Header("Tilt (Z-rotation)")]
    [SerializeField] private float tiltAngle = 8f;        // ������������ ������ �� Z (� ��������)
    [SerializeField] private float tiltDuration = 0.4f;   // ����� �������� � ���� �������

    // ���������� ����� (����� ��������� ������� ��� Disable)
    private Sequence jumpSequence;
    private Tween tiltTween;

    // ���������� ��������� �������, ����� ������ ������������ ����� ����
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
        // ����� �����: ������ �����, ����� DOTween ����� �������� ����������� ��� ����������� ������
        if (jumpSequence != null && jumpSequence.IsActive()) jumpSequence.Kill();
        if (tiltTween != null && tiltTween.IsActive()) tiltTween.Kill();

        // ���������� ������ � �������� ���������
        target.anchoredPosition = startAnchoredPos;
        target.localEulerAngles = startEulerAngles;
    }

    private void PlayJumpLoop()
    {
        // ������ ����� ������������������ ������
        jumpSequence = DOTween.Sequence();

        // ���� �����: ������ ������������ �����
        jumpSequence.Append(
            target.DOAnchorPosY(startAnchoredPos.y + jumpHeight, jumpDuration * 0.4f)
                  .SetEase(Ease.OutQuad) // ������ �������� � ����������� � ����
        );

        // ���� ����: ������ ������� � �������� (bounce)
        jumpSequence.Append(
            target.DOAnchorPosY(startAnchoredPos.y, jumpDuration * 1)
                  .SetEase(Ease.OutBounce) // ��� ����� "��������"
        );

        // ������ ����������� ����
        jumpSequence.SetLoops(-1, LoopType.Restart);

        // �����������: ����� �������� ������������ ���� ��� ����� ���� (UI ������ ��� �����)
        // jumpSequence.SetUpdate(true);
    }

    private void PlayTiltLoop()
    {
        // ������ ������ ����-���� �� ��� Z
        // Yoyo = �����-�����-�����...
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

        // ���� ���� �����, ����� ��� �������� � ����� � ��������������:
        // tiltTween.SetUpdate(true);
    }
}
