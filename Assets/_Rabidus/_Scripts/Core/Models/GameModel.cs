using System.Collections.Generic;
using UnityEngine;

public class GameModel : IGameModel
{
    public bool IsLoaded { get; private set; }
    public IGameConfig Config { get; private set; }
    public List<Vector3Int> OpenedCells { get; private set; } = new List<Vector3Int>();

    public GameModel(IGameConfig config)
    {
        Config = config;

        foreach (var item in Config.StartCells)
        {
            OpenedCells.Add(item.Key);
        }
    }

    public void LoadData(object data)
    {
        if (data != null)
        {
            var loadedCells = ((GameSaveData)data).OpenedCells;

            foreach (var item in loadedCells)
            {
                if (OpenedCells.Contains(item)) continue;

                OpenedCells.Add(item);
            }
        }

        IsLoaded = true;
    }
}

public interface IGameModel : ILoadableModel
{
    public IGameConfig Config { get; }
    public List<Vector3Int> OpenedCells { get; }
}

