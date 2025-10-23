public class GameModel : IGameModel
{
    public IGameConfig Config { get; private set; }

    public GameModel(IGameConfig config)
    {
        Config = config;
    }
}

public interface IGameModel 
{
    public IGameConfig Config { get; }
}

