public class SoundViewModel : ISoundViewModel
{
    public ReactiveProperty<bool> Sound { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> Music { get; private set; } = new ReactiveProperty<bool>();

    private ISoundModel _model;
    private ISaveService _saveService;

    public SoundViewModel(ISoundModel model, ISaveService saveService)
    {
        _saveService = saveService;
        _model = model;

        Sound.Value = _model.Sound;
        Music.Value = _model.Music;
    }

    public void ToggleMusic()
    {
        _model.ToggleMusic();
        Music.Value = _model.Music;

        _saveService.SaveSound();
    }

    public void ToggleSound()
    {
        _model.ToggleSound();
        Sound.Value = _model.Sound;

        _saveService.SaveSound();
    }
}

public interface ISoundViewModel
{
    public ReactiveProperty<bool> Sound { get; }
    public ReactiveProperty<bool> Music { get; }
    public void ToggleSound();
    public void ToggleMusic();
}
