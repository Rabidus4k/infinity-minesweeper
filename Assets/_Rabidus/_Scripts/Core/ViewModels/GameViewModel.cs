using System.Collections.Generic;
using UnityEngine;

public class GameViewModel : IGameViewModel
{
    public ReactiveProperty<bool> IsLoaded { get; private set; } = new ReactiveProperty<bool>();
    public ReactiveProperty<IGameConfig> Config { get; private set; } = new ReactiveProperty<IGameConfig>();
    public ReactiveProperty<List<Vector3Int>> OpenedCells { get; private set; } = new ReactiveProperty<List<Vector3Int>>();


    protected IGameModel _model;

    public GameViewModel(IGameModel model)
    {
        _model = model;

        OpenedCells.Value = _model.OpenedCells;
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
    public ReactiveProperty<List<Vector3Int>> OpenedCells { get; }
}