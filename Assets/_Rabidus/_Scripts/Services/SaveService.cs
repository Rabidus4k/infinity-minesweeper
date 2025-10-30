public class SaveService : ISaveService
{
    private readonly ICurrencyModel _currency;
    private readonly IScoreModel _score;
    private readonly IGameModel _game;
    private readonly ICoordsModel _coords;
    private readonly IAppearenceModel _appearence;

    public SaveService
    (
        ICurrencyModel currency,
        IScoreModel score,
        IGameModel game,
        ICoordsModel coords,
        IAppearenceModel appearence
    )
    {
        _currency = currency;
        _score = score;
        _game = game;
        _coords = coords;
        _appearence = appearence;

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

    public void Load()
    {
        SaveSystem.TryLoad(_currency);
        SaveSystem.TryLoad(_score);
        SaveSystem.TryLoad(_game);
        SaveSystem.TryLoad(_coords);
        SaveSystem.TryLoad(_appearence);
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

    void Load();
    void ResetSaves();
}