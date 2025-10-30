using MirraGames.SDK;
using UnityEngine;

public static class SaveSystem
{
    private static string SaveKeyCurrency = "SaveKeyCurrency";
    private static string SaveKeyScore = "SaveKeyScore";
    private static string SaveKeyGame = "SaveKeyGame";
    private static string SaveKeyCoords = "SaveKeyCoords";
    private static string SaveKeyAppearence = "SaveKeyAppearence";
    private static string SaveKeySound = "SaveKeySound";

    public static void Save(ICurrencyModel model)
    {
        CurrencySaveData data = new CurrencySaveData(model);
        MirraSDK.Data.SetObject<CurrencySaveData>(SaveKeyCurrency, data, important: true);
        Debug.Log($"[SaveSystem] Save: ICurrencyModel");
    }

    public static void Save(IScoreModel model)
    {
        ScoreSaveData data = new ScoreSaveData(model);
        MirraSDK.Data.SetObject<ScoreSaveData>(SaveKeyScore, data, important: true);
        Debug.Log($"[SaveSystem] Save: IScoreModel");
    }

    public static void Save(IGameModel model)
    {
        GameSaveData data = new GameSaveData(model);
        MirraSDK.Data.SetObject<GameSaveData>(SaveKeyGame, data, important: true);
        Debug.Log($"[SaveSystem] Save: IGameModel");
    }

    public static void Save(ICoordsModel model)
    {
        CoordsSaveData data = new CoordsSaveData(model);
        MirraSDK.Data.SetObject<CoordsSaveData>(SaveKeyCoords, data, important: true);
        Debug.Log($"[SaveSystem] Save: ICoordsModel");
    }

    public static void Save(IAppearenceModel model)
    {
        AppearenceSaveData data = new AppearenceSaveData(model);
        MirraSDK.Data.SetObject<AppearenceSaveData>(SaveKeyAppearence, data, important: true);
        Debug.Log($"[SaveSystem] Save: IAppearenceModel");
    }

    public static void Save(ISoundModel model)
    {
        SoundSaveData data = new SoundSaveData(model);
        MirraSDK.Data.SetObject<SoundSaveData>(SaveKeySound, data, important: true);
        Debug.Log($"[SaveSystem] Save: ISoundModel");
    }

    public static bool TryLoad(ICurrencyModel model)
    {
        if (!MirraSDK.Data.HasKey(SaveKeyCurrency)) return false;

        CurrencySaveData data = MirraSDK.Data.GetObject<CurrencySaveData>(SaveKeyCurrency);
        Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: ICurrencyModel");

        return true;
    }

    public static bool TryLoad(IScoreModel model)
    {
        if (!MirraSDK.Data.HasKey(SaveKeyScore)) return false;

        ScoreSaveData data = MirraSDK.Data.GetObject<ScoreSaveData>(SaveKeyScore);
        Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: IScoreModel");

        return true;
    }

    public static bool TryLoad(IGameModel model)
    {
        if (!MirraSDK.Data.HasKey(SaveKeyGame)) return false;

        GameSaveData data = MirraSDK.Data.GetObject<GameSaveData>(SaveKeyGame);
        Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: IGameModel");

        return true;
    }

    public static bool TryLoad(ICoordsModel model)
    {
        if (!MirraSDK.Data.HasKey(SaveKeyCoords)) return false;

        CoordsSaveData data = MirraSDK.Data.GetObject<CoordsSaveData>(SaveKeyCoords);
        Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: ICoordsModel");

        return true;
    }

    public static bool TryLoad(IAppearenceModel model)
    {
        if (!MirraSDK.Data.HasKey(SaveKeyAppearence)) return false;

        AppearenceSaveData data = MirraSDK.Data.GetObject<AppearenceSaveData>(SaveKeyAppearence);
        Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: IAppearenceModel");

        return true;
    }

    public static bool TryLoad(ISoundModel model)
    {
        if (!MirraSDK.Data.HasKey(SaveKeySound)) return false;

        SoundSaveData data = MirraSDK.Data.GetObject<SoundSaveData>(SaveKeySound);
        Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: ISoundModel");

        return true;
    }

    public static void ResetSaves()
    {
        MirraSDK.Data.DeleteAll();
    }
}
