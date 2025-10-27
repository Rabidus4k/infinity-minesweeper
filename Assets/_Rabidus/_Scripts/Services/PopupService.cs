using Lean.Pool;
using UnityEngine;

public class PopupService: IPopupService
{
    private Popup _popupPrefab;
    private IPopupConfig _config;

    public PopupService(Popup popup, IPopupConfig config)
    {
        _config = config;
        _popupPrefab = popup;
    }

    public Popup SpawnPopup(Vector3 position, Quaternion rotation, Transform parent)
    {
        var popup = LeanPool.Spawn(_popupPrefab);

        popup.transform.SetParent(parent);
        popup.transform.localPosition = position;
        popup.transform.rotation = rotation;

        return popup;
    }
}

public interface IPopupService
{
    Popup SpawnPopup(Vector3 position, Quaternion rotation, Transform parent);
}