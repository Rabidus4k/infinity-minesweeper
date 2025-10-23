public class GameViewModel : IGameViewModel
{
    public ReactiveProperty<IGameConfig> Config { get; private set; } = new ReactiveProperty<IGameConfig>();

    protected IGameModel _model;
    public GameViewModel(IGameModel model)
    {
        _model = model;

        Config.Value = _model.Config;
    }
}

public interface IGameViewModel
{
    public ReactiveProperty<IGameConfig> Config { get; }
}