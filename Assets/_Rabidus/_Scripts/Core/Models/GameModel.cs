using System.Collections.Generic;
using UnityEngine;

public class GameModel : IGameModel
{
    public IGameConfig Config { get; private set; }
    public Dictionary<Vector3Int, CellInfo> Cells { get; private set; } = new Dictionary<Vector3Int, CellInfo>();

    public GameModel(IGameConfig config)
    {
        Config = config;

        foreach (var item in Config.StartCells)
        {
            Cells.Add(item.Key, item.Value);
        }
    }
}

public interface IGameModel
{
    public IGameConfig Config { get; }
    public Dictionary<Vector3Int, CellInfo> Cells { get; }
}

