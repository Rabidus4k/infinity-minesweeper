public class AppearenceViewModel : IAppearenceViewModel
{
    public ReactiveProperty<bool> IsLoaded { get; private set; } = new ReactiveProperty<bool>();

    public ReactiveProperty<ThemeConfig> ThemeConfig { get; private set; } = new ReactiveProperty<ThemeConfig>();

    private IAppearenceModel _model;
    
    public AppearenceViewModel(IAppearenceModel model)
    {
        _model = model;

        ThemeConfig.Value = _model.ThemeConfig;
    } 

    public void ChangeTheme(ThemeConfig theme)
    {
        _model.ChangeTheme(theme);
        ThemeConfig.Value = theme;
    }

    public void LoadData(object data)
    {
        _model.LoadData(data);
        IsLoaded.Value = _model.IsLoaded;
    }
}

public interface IAppearenceViewModel : ILoadableViewModel
{
    public ReactiveProperty<ThemeConfig> ThemeConfig { get; }
    public void ChangeTheme(ThemeConfig theme);
}
