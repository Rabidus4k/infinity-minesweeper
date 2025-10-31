public class AppearenceModel : IAppearenceModel
{
    public bool IsLoaded { get; private set; }
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
        if (data != null)
            ThemeConfig = ((AppearenceSaveData)data).ThemeConfig;

        IsLoaded = true;
    }
}

public interface IAppearenceModel : ILoadableModel
{
    public ThemeConfig ThemeConfig { get; }
    public void ChangeTheme(ThemeConfig theme);
}
