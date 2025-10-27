using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NotificationMessage
{
    [TextArea]
    public string Message;
    public float Timer = 3f;
}

[CreateAssetMenu(menuName = "Rabidus/Notification")]
public class UINotificationInfo : ScriptableObject
{
    public bool IgnoreTimer = false;
    public float Delay = 0.4f;
    public List<NotificationMessage> Messages = new List<NotificationMessage>();
}
