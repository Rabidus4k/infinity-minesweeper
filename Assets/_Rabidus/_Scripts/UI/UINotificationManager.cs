using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINotificationManager : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private UINotification _notificationPrefab;
    [SerializeField] private float _noRepeatTime = 10f;
    [SerializeField] private int _maxNotificationMessages = 10;

    private Dictionary<UINotificationInfo, float> _currentNotifications =new Dictionary<UINotificationInfo, float>();
    private List<UINotification> _notifications = new List<UINotification>();

    public void SendNotification(UINotificationInfo info)
    {
        SendNotificationAsync(info).Forget();
    }

    public void CloseNotification(UINotificationInfo info)
    {
        foreach (var message in info.Messages)
        {
            var findNotification = _notifications.Find(x => x.Message == message);
            if (findNotification != null)
                HideNotificationAsync(findNotification, 0).Forget();
        }
    }

    private async UniTask SendNotificationAsync(UINotificationInfo info)
    {
        if (!info.IgnoreTimer && !CheckNotification(info))
            return;

        foreach (var item in info.Messages)
        {
            if (_notifications.Count > _maxNotificationMessages)
                HideNotificationAsync(_notifications[0], 0).Forget();

            var notificationInstance = Instantiate(_notificationPrefab, _container);
            notificationInstance.Initialize(item);
            _notifications.Add(notificationInstance);
            HideNotificationAsync(notificationInstance, item.Timer).Forget();

            LayoutRebuilder.ForceRebuildLayoutImmediate(_container);

            await UniTask.WaitForSeconds(info.Delay);
        }
    }

    private void HideNotification(UINotificationInfo info)
    {

    }

    private async UniTask HideNotificationAsync(UINotification notificationInstance, float time)
    {
        await UniTask.WaitForSeconds(time);

        if(notificationInstance != null)
        {
            _notifications.Remove(notificationInstance);
            Destroy(notificationInstance.gameObject);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_container);
        }
    }

    private bool CheckNotification(UINotificationInfo info)
    {
        if (info == null) return false;

        if (_currentNotifications.ContainsKey(info))
        {
            if ((Time.time - _currentNotifications[info]) > _noRepeatTime)
            {
                _currentNotifications[info] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            _currentNotifications.Add(info, Time.time);
            return true;
        }
    }
}
