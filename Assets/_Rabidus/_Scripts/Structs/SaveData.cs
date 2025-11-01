using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public CurrencySaveData CurrencyData;
    public ScoreSaveData ScoreData;
    public GameSaveData GameData;
    public CoordsSaveData CoordsDate;
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
public class CoordsSaveData
{
    public List<CoordsInfo> CoordsInfo;

    public CoordsSaveData() { }

    public CoordsSaveData(ICoordsModel model)
    {
        CoordsInfo = model.CoordsInfo;
    }
}

[Serializable]
public class GameSaveData
{
    public List<Vector3Int> OpenedCells = new List<Vector3Int>();

    public GameSaveData() { }

    public GameSaveData(IGameModel model)
    {
        OpenedCells = model.OpenedCells;
    }
}

[Serializable]
public class AppearenceSaveData
{
    public ThemeConfig ThemeConfig;

    public AppearenceSaveData() { }

    public AppearenceSaveData(IAppearenceModel model)
    {
        ThemeConfig = model.ThemeConfig;
    }
}

[Serializable]
public class SoundSaveData 
{
    public bool Sound;
    public bool Music;

    public SoundSaveData() { }

    public SoundSaveData(ISoundModel model)
    {
        Sound = model.Sound;
        Music = model.Music;
    }
}


[Serializable]
public class CellEntry
{
    public Vector3Int Key;
    public CellInfo Value;
}

[Serializable]
public class CoordsInfo
{
    public string Name;
    public Vector3 Coords;
}
