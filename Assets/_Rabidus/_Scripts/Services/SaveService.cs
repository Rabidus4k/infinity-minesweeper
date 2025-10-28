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

    public void Save()
    {
        SaveSystem.Save(_currency, _score, _game, _coords);
    }

    public bool Load()
    {
        return SaveSystem.TryLoad(_currency, _score, _game, _coords);
    }

    public void ResetSaves()
    {
        SaveSystem.ResetSaves();
    }
}
public interface ISaveService
{
    void Save();
    bool Load();
    void ResetSaves();
}