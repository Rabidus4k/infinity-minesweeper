public class SaveService : ISaveService
{
    private readonly ICurrencyModel _currency;
    private readonly IScoreModel _score;
    private readonly IGameModel _game;
    private readonly ICoordsModel _coords;

    public SaveService(ICurrencyModel currency, IScoreModel score, IGameModel game, ICoordsModel coords)
    {
        _currency = currency;
        _score = score;
        _game = game;
        _coords = coords;

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

    public void Load()
    {
        SaveSystem.TryLoad(_currency);
        SaveSystem.TryLoad(_score);
        SaveSystem.TryLoad(_game);
        SaveSystem.TryLoad(_coords);
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

    void Load();
    void ResetSaves();
}