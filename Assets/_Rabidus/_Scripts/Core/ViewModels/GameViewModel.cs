using System.Collections.Generic;
using UnityEngine;

public class GameViewModel : IGameViewModel
{
    public ReactiveProperty<bool> IsLoaded { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<IGameConfig> Config { get; private set; } = new ReactiveProperty<IGameConfig>();

    public ReactiveProperty<Dictionary<Vector3Int, CellInfo>> Cells { get; private set; } = new ReactiveProperty<Dictionary<Vector3Int, CellInfo>>();


    protected IGameModel _model;

    public GameViewModel(IGameModel model)
    {
        _model = model;

        Cells.Value = _model.Cells;
        Config.Value = _model.Config;
    }

    public void LoadData(object data)
    {
        _model.LoadData(data);
        IsLoaded.Value = _model.IsLoaded;
    }
}

public interface IGameViewModel : ILoadableViewModel
{
    public ReactiveProperty<IGameConfig> Config { get; }
    public ReactiveProperty<Dictionary<Vector3Int, CellInfo>> Cells { get; }
}