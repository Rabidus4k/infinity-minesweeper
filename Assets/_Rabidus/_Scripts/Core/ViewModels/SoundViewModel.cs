public class SoundViewModel : ISoundViewModel
{
    public ReactiveProperty<bool> IsLoaded { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> Sound { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> Music { get; private set; } = new ReactiveProperty<bool>();

    private ISoundModel _model;

    public SoundViewModel(ISoundModel model)
    {
        _model = model;

        Sound.Value = _model.Sound;
        Music.Value = _model.Music;
    }

    public void ToggleMusic()
    {
        _model.ToggleMusic();
        Music.Value = _model.Music;
    }

    public void ToggleSound()
    {
        _model.ToggleSound();
        Sound.Value = _model.Sound;
    }

    public void LoadData(object data)
    {
        _model.LoadData(data);
        IsLoaded.Value = _model.IsLoaded;
    }
}

public interface ISoundViewModel : ILoadableViewModel
{
    public ReactiveProperty<bool> Sound { get; }
    public ReactiveProperty<bool> Music { get; }
    public void ToggleSound();
    public void ToggleMusic();
}
