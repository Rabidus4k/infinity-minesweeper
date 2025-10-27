using MirraGames.SDK;
using UnityEngine;

public static class SaveSystem
{
    private static string SaveKey = "saveData";

    public static void Save(ICurrencyModel currency, IScoreModel score, IGameModel game)
    {
        var data = new SaveData
        {
            CurrencyData = new CurrencySaveData(currency),
            ScoreData = new ScoreSaveData(score),
            GameData = new GameSaveData(game)
        };

        MirraSDK.Data.SetObject<SaveData>(SaveKey, data, important: true);
        
        Debug.Log($"[SaveSystem] Saved");
    }

    public static bool TryLoad(ICurrencyModel currency, IScoreModel score, IGameModel game)
    {
        if (!MirraSDK.Data.HasKey(SaveKey)) return false;

        SaveData data = MirraSDK.Data.GetObject<SaveData>(SaveKey);
        Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        if (data.CurrencyData != null)
        {
            currency.LoadData(data.CurrencyData);
        }

        if (data.ScoreData != null)
        {
            score.LoadData(data.ScoreData);
        }

        if (data.GameData != null)
        {
            game.LoadData(data.GameData);
        }

        Debug.Log($"[SaveSystem] Loaded ");

        return true;
    }

    public static void ResetSaves()
    {
        MirraSDK.Data.DeleteKey(SaveKey);
    }
}
