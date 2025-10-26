using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    [SerializeField] protected CanvasGroup _canvasGroup;
    [SerializeField] protected bool _hideOnAwake = false;

    protected bool _isShown = false;
    public bool IsShown => _isShown;


    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        if (_hideOnAwake)
            HidePanel();
    }

    [Button]
    public virtual void ShowPanel()
    {
        _isShown = true;
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
    }

    [Button]
    public virtual void HidePanel()
    {
        _isShown = false;
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
    }

    [Button]
    public void TogglePanel()
    {
        if (_isShown)
            HidePanel();
        else
            ShowPanel();
    }
}
