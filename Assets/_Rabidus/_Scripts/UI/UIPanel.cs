using DG.Tweening;
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
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        _isShown = true;
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
    }

    public virtual void ShowPanel(float time)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        _isShown = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1.0f, time);
    }

    [Button]
    public virtual void HidePanel()
    {
        _isShown = false;
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
    }

    public virtual void HidePanel(float time)
    {
        _isShown = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, time);
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
