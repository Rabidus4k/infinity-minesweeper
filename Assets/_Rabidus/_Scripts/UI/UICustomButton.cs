using UnityEngine;
using UnityEngine.UI;

public abstract class UICustomButton : MonoBehaviour
{
    [SerializeField] protected Button _button;

    protected virtual void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();
    }

    protected virtual void OnEnable() => _button.onClick.AddListener(HandleClick);

    protected virtual void OnDisable() => _button.onClick.RemoveListener(HandleClick);

    protected abstract void HandleClick();
}
