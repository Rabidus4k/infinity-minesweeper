public class SaveService : ISaveService
{
    private readonly ICurrencyModel _currency;
    private readonly IScoreModel _score;
    private readonly IGameModel _game;

    public SaveService(ICurrencyModel currency, IScoreModel score, IGameModel game)
    {
        _currency = currency;
        _score = score;
        _game = game;

        Load();
    }

    public void Save()
    {
        SaveSystem.Save(_currency, _score, _game);
    }

    public bool Load()
    {
        return SaveSystem.TryLoad(_currency, _score, _game);
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