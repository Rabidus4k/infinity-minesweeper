public class SaveService : ISaveService
{
    private readonly ICurrencyModel _currency;
    private readonly IScoreModel _score;
    private readonly IGameModel _game;
    private readonly ICoordsModel _coords;
    private readonly IAppearenceModel _appearence;
    private readonly ISoundModel _sound;

    public SaveService
    (
        ICurrencyModel currency,
        IScoreModel score,
        IGameModel game,
        ICoordsModel coords,
        IAppearenceModel appearence,
        ISoundModel sound
    )
    {
        _currency = currency;
        _score = score;
        _game = game;
        _coords = coords;
        _appearence = appearence;
        _sound = sound;

        Load();
    }

    public void SaveCurrency()
    {
        SaveSystem.Save(_currency);
    }

    public void SaveScore()
    {
        SaveSystem.Save(_score);
    }

    public void SaveGame()
    {
        SaveSystem.Save(_game);
    }

    public void SaveCoords()
    {
        SaveSystem.Save(_coords);
    }

    public void SaveAppearence()
    {
        SaveSystem.Save(_appearence);
    }

    public void SaveSound()
    {
        SaveSystem.Save(_sound);
    }

    public void Load()
    {
        SaveSystem.TryLoad(_currency);
        SaveSystem.TryLoad(_score);
        SaveSystem.TryLoad(_game);
        SaveSystem.TryLoad(_coords);
        SaveSystem.TryLoad(_appearence);
        SaveSystem.TryLoad(_sound);
    }

    public void ResetSaves()
    {
        SaveSystem.ResetSaves();
    }
}

public interface ISaveService
{
    void SaveCurrency();
    void SaveScore();
    void SaveGame();
    void SaveCoords();
    void SaveAppearence();
    void SaveSound();
    void Load();
    void ResetSaves();
}