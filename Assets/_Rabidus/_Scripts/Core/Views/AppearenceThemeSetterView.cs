using System;
using UnityEngine;
using Zenject;

public abstract class AppearenceThemeSetterView : MonoBehaviour
{
    [SerializeField] protected int _index;
    protected IAppearenceViewModel _viewModel;

    [Inject]
    protected void Construct(IAppearenceViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.ThemeConfig.OnChanged += ApplyTheme;

        Initialize();
        ApplyTheme(_viewModel.ThemeConfig.Value);
    }

    protected virtual void OnDisable()
    {
        _viewModel.ThemeConfig.OnChanged -= ApplyTheme;
    }

    protected abstract void Initialize();
    protected abstract void ApplyTheme(ThemeConfig theme);
}
