using UnityEngine;

public class GameViewModel : IGameViewModel
{
    public ReactiveProperty<IGameConfig> Config { get; private set; } = new ReactiveProperty<IGameConfig>();
    public ReactiveProperty<Cell> LastSelectCell { get; private set; } = new ReactiveProperty<Cell>();

    protected IGameModel _model;
    public GameViewModel(IGameModel model)
    {
        _model = model;

        Config.Value = _model.Config;
    }

    public void SelectCell(Cell cell)
    {
        LastSelectCell.Value = cell;
    }
}

public interface IGameViewModel
{
    public ReactiveProperty<IGameConfig> Config { get; }
    public ReactiveProperty<Cell> LastSelectCell { get; }
    void SelectCell(Cell cell);
}