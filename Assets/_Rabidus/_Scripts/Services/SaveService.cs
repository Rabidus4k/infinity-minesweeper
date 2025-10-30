using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class SaveService : ISaveService
{
    private readonly ICurrencyModel _currency;
    private readonly IScoreModel _score;
    private readonly IGameModel _game;
    private readonly ICoordsModel _coords;
    private readonly IAppearenceModel _appearence;
    private readonly ISoundModel _sound;

    public ReactiveProperty<bool> IsLoaded { get; private set; } = new ReactiveProperty<bool>();

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

    public async UniTask Load()
    {
        await SaveSystem.TryLoad(_currency);
        await SaveSystem.TryLoad(_score);
        await SaveSystem.TryLoad(_game);
        await SaveSystem.TryLoad(_coords);
        await SaveSystem.TryLoad(_appearence);
        await SaveSystem.TryLoad(_sound);

        IsLoaded.Value = true;
    }

    public void ResetSaves()
    {
        SaveSystem.ResetSaves().Forget();
    }
}

public interface ISaveService
{
    public ReactiveProperty<bool> IsLoaded { get; }
    void SaveCurrency();
    void SaveScore();
    void SaveGame();
    void SaveCoords();
    void SaveAppearence();
    void SaveSound();
    UniTask Load();
    void ResetSaves();
}