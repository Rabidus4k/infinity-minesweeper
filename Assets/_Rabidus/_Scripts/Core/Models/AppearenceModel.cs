using UnityEngine;

public class AppearenceModel : IAppearenceModel
{
    public ThemeConfig ThemeConfig { get; private set; }

    public AppearenceModel(ThemeConfig defaultThemeConfig)
    {
        ThemeConfig = defaultThemeConfig;
    }

    public void ChangeTheme(ThemeConfig theme)
    {
        ThemeConfig = theme;
    }

    public void LoadData(object data)
    {
        ThemeConfig = ((AppearenceSaveData)data).ThemeConfig;
    }
}

public interface IAppearenceModel : ILoadable
{
    public ThemeConfig ThemeConfig { get; }
    public void ChangeTheme(ThemeConfig theme);
}
