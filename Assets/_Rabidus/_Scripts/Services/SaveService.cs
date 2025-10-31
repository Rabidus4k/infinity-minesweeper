using Cysharp.Threading.Tasks;

public class SaveService : ISaveService
{
    private readonly ICurrencyViewModel _currencyViewModel;
    private readonly IScoreViewModel _scoreViewModel;
    private readonly IGameViewModel _gameViewModel;
    private readonly ICoordsViewModel _coordsViewModel;
    private readonly IAppearenceViewModel _appearenceViewModel;
    private readonly ISoundViewModel _soundViewModel;

    private readonly ICurrencyModel _currencyModel;
    private readonly IScoreModel _scoreModel;
    private readonly IGameModel _gameModel;
    private readonly ICoordsModel _coordsModel;
    private readonly IAppearenceModel _appearenceModel;
    private readonly ISoundModel _soundModel;

    public SaveService
    (
        ICurrencyViewModel currencyViewModel,
        IScoreViewModel scoreViewModel,
        IGameViewModel gameViewModel,
        ICoordsViewModel coordsViewModel,
        IAppearenceViewModel appearenceViewModel,
        ISoundViewModel soundViewModel,

        ICurrencyModel currencyModel,
        IScoreModel scoreModel,
        IGameModel gameModel,
        ICoordsModel coordsModel,
        IAppearenceModel appearenceModel,
        ISoundModel soundModel
    )
    {
        _currencyViewModel = currencyViewModel;
        _scoreViewModel = scoreViewModel;
        _gameViewModel = gameViewModel;
        _coordsViewModel = coordsViewModel;
        _appearenceViewModel = appearenceViewModel;
        _soundViewModel = soundViewModel;

        _currencyModel = currencyModel;
        _scoreModel = scoreModel;
        _gameModel = gameModel;
        _coordsModel = coordsModel;
        _appearenceModel = appearenceModel;
        _soundModel = soundModel;
    }

    public async UniTask SaveCurrency()
    {
        await SaveSystem.Save(_currencyModel);
    }

    public async UniTask SaveScore()
    {
        await SaveSystem.Save(_scoreModel);
    }

    public async UniTask SaveGame()
    {
        await SaveSystem.Save(_gameModel);
    }

    public async UniTask SaveCoords()
    {
        await SaveSystem.Save(_coordsModel);
    }

    public async UniTask SaveAppearence()
    {
        await SaveSystem.Save(_appearenceModel);
    }

    public async UniTask SaveSound()
    {
        await SaveSystem.Save(_soundModel);
    }

    public async UniTask Load()
    {
        await SaveSystem.TryLoad(_currencyViewModel);
        await SaveSystem.TryLoad(_scoreViewModel);
        await SaveSystem.TryLoad(_gameViewModel);
        await SaveSystem.TryLoad(_coordsViewModel);
        await SaveSystem.TryLoad(_appearenceViewModel);
        await SaveSystem.TryLoad(_soundViewModel);
    }

    public async UniTask ResetSaves()
    {
        await SaveSystem.ResetSaves();
    }
}

public interface ISaveService
{
    UniTask SaveCurrency();
    UniTask SaveScore();
    UniTask SaveGame();
    UniTask SaveCoords();
    UniTask SaveAppearence();
    UniTask SaveSound();
    UniTask Load();
    UniTask ResetSaves();
}