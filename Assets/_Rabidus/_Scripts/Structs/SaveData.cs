using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public CurrencySaveData CurrencyData;
    public ScoreSaveData ScoreData;
    public GameSaveData GameData;
}

[Serializable]
public class CurrencySaveData
{
    public int Gems;

    public CurrencySaveData() { }

    public CurrencySaveData(ICurrencyModel model)
    {
        Gems = model.Gems;
    }
}

[Serializable]
public class ScoreSaveData
{
    public int MaxScore;

    public ScoreSaveData() { }

    public ScoreSaveData(IScoreModel model)
    {
        MaxScore = model.MaxScore;
    }
}

[Serializable]
public class GameSaveData
{
    public List<CellEntry> Cells = new List<CellEntry>();

    public GameSaveData() { }

    public GameSaveData(IGameModel model)
    {
        foreach (var kvp in model.Cells)
        {
            Cells.Add(new CellEntry
            {
                Key = kvp.Key,
                Value = kvp.Value
            });
        }
    }
}

[Serializable]
public class CellEntry
{
    public Vector3Int Key;
    public CellInfo Value;
}