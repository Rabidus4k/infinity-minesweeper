using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIAppearenceThemeChanger : UICustomButton
{
    [SerializeField] private List<ThemeConfig> _themes = new List<ThemeConfig>();

    private int _currentThemeIndex = 0;

    private IAppearenceViewModel _viewModel;

    [Inject]
    private void Construct(IAppearenceViewModel appearenceViewModel)
    {
        _viewModel = appearenceViewModel;

        _currentThemeIndex = _themes.IndexOf(_viewModel.ThemeConfig.Value);
    }

    protected override void HandleClick()
    {
        _currentThemeIndex++;

        if (_currentThemeIndex >= _themes.Count)
            _currentThemeIndex = 0;

        _viewModel.ChangeTheme(_themes[_currentThemeIndex]);
    }
}
