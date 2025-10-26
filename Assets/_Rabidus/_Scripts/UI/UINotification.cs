using UnityEngine;
using UnityEngine.UI;

public class UINotification : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _notificationText;

    private RectTransform _rect;
    private NotificationMessage _message;

    public NotificationMessage Message => _message;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void Initialize(NotificationMessage message)
    {
        _message = message;
        _notificationText.SetText($"{message.Message}");
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
    }
}
