public class AppearenceViewModel : IAppearenceViewModel
{
    public ReactiveProperty<ThemeConfig> ThemeConfig { get; private set; } = new ReactiveProperty<ThemeConfig>();

    private IAppearenceModel _model;
    private ISaveService _saveService;

    public AppearenceViewModel(IAppearenceModel model, ISaveService saveService)
    {
        _saveService = saveService;
        _model = model;

        ThemeConfig.Value = _model.ThemeConfig;
    } 

    public void ChangeTheme(ThemeConfig theme)
    {
        _model.ChangeTheme(theme);
        ThemeConfig.Value = theme;

        //_saveService.SaveAppearence();
    }
}

public interface IAppearenceViewModel
{
    public ReactiveProperty<ThemeConfig> ThemeConfig { get; }
    public void ChangeTheme(ThemeConfig theme);
}
