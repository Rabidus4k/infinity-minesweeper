using Cysharp.Threading.Tasks;
using MirraGames.SDK;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Unity.Services.CloudSave;
using UnityEngine;

public static class SaveSystem
{
    private static string SaveKeyCurrency = "SaveKeyCurrency";
    private static string SaveKeyScore = "SaveKeyScore";
    private static string SaveKeyGame = "SaveKeyGame";
    private static string SaveKeyCoords = "SaveKeyCoords";
    private static string SaveKeyAppearence = "SaveKeyAppearence";
    private static string SaveKeySound = "SaveKeySound";

    public static async UniTask Save(ICurrencyModel model)
    {
        CurrencySaveData data = new CurrencySaveData(model);
        MirraSDK.Data.SetObject<CurrencySaveData>(SaveKeyCurrency, data, important: true);
        Debug.Log($"[SaveSystem] Save: ICurrencyModel");

        //await SaveObjectDataAsync(SaveKeyCurrency, data);
    }

    public static async UniTask Save(IScoreModel model)
    {
        ScoreSaveData data = new ScoreSaveData(model);
        MirraSDK.Data.SetObject<ScoreSaveData>(SaveKeyScore, data, important: true);
        Debug.Log($"[SaveSystem] Save: IScoreModel");

        //await SaveObjectDataAsync(SaveKeyScore, data);
    }

    public static async UniTask Save(IGameModel model)
    {
        GameSaveData data = new GameSaveData(model);

        string stringData = Payload.ToString(model.OpenedCells);

        long bits1 = Encoding.Unicode.GetByteCount(stringData) * 8L;
        long bits2 = (long)stringData.Length * sizeof(char) * 8L;

        MirraSDK.Data.SetString(SaveKeyGame, stringData, important: true);
        Debug.Log($"[SaveSystem] Save: IGameModel");
        Debug.Log(stringData);
        Debug.Log($"Битов (только символы): {bits1} (проверка: {bits2})");
        Debug.Log($"Это примерно {bits1 / 8} байт.");
        //await SaveObjectDataAsync(SaveKeyGame, data);
    }

    public static async UniTask Save(ICoordsModel model)
    {
        CoordsSaveData data = new CoordsSaveData(model);
        MirraSDK.Data.SetObject<CoordsSaveData>(SaveKeyCoords, data, important: true);
        Debug.Log($"[SaveSystem] Save: ICoordsModel");

        //await SaveObjectDataAsync(SaveKeyCoords, data);
    }

    public static async UniTask Save(IAppearenceModel model)
    {
        AppearenceSaveData data = new AppearenceSaveData(model);
        MirraSDK.Data.SetObject<AppearenceSaveData>(SaveKeyAppearence, data, important: true);
        Debug.Log($"[SaveSystem] Save: IAppearenceModel");

        //await SaveObjectDataAsync(SaveKeyAppearence, data);
    }

    public static async UniTask Save(ISoundModel model)
    {
        SoundSaveData data = new SoundSaveData(model);
        MirraSDK.Data.SetObject<SoundSaveData>(SaveKeySound, data, important: true);
        Debug.Log($"[SaveSystem] Save: ISoundModel");

        //await SaveObjectDataAsync(SaveKeySound, data);
    }

    private static async UniTask SaveObjectDataAsync(string key, object data)
    {
        if (!MirraSDK.Data.HasKey(key)) return;

        try
        {
            Dictionary<string, object> oneElement = new Dictionary<string, object>
                {
                    { key, data }
                };

            Dictionary<string, string> result =
                await CloudSaveService.Instance.Data.Player.SaveAsync(oneElement);
            string writeLock = result[key];

            Debug.Log($"Successfully saved {key}:{data} with updated write lock {writeLock}");
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

    }

    public static async UniTask TryLoad(ICurrencyViewModel model)
    {
        if (MirraSDK.Data.HasKey(SaveKeyCurrency))
        {
            CurrencySaveData data = MirraSDK.Data.GetObject<CurrencySaveData>(SaveKeyCurrency);
            model.LoadData(data);
        }
        else
        {
            model.LoadData(null);
        }

        Debug.Log($"[SaveSystem] Loaded: ICurrencyModel");
    }

    public static async UniTask TryLoad(IScoreViewModel model)
    {
        if (MirraSDK.Data.HasKey(SaveKeyScore))
        {
            ScoreSaveData data = MirraSDK.Data.GetObject<ScoreSaveData>(SaveKeyScore);
            model.LoadData(data);
        }
        else
        {
            model.LoadData(null);
        }

        Debug.Log($"[SaveSystem] Loaded: IScoreModel");
    }

    public static async UniTask TryLoad(IGameViewModel model)
    {
        if (MirraSDK.Data.HasKey(SaveKeyGame))
        {
            string json = MirraSDK.Data.GetString(SaveKeyGame);
            var payload = Payload.FromString(json);

            GameSaveData data = new GameSaveData();

            foreach (var cell in payload)
            {
                data.OpenedCells.Add(cell);
            }
            Debug.Log($"[SaveSystem] Loaded: IGameModel {json}");
            model.LoadData(data);
        }
        else
        {
            Debug.Log($"[SaveSystem] Loaded: IGameModel");
            model.LoadData(null);
        }
    }

    public static async UniTask TryLoad(ICoordsViewModel model)
    {
        if (MirraSDK.Data.HasKey(SaveKeyCoords))
        {
            CoordsSaveData data = MirraSDK.Data.GetObject<CoordsSaveData>(SaveKeyCoords);
            model.LoadData(data);
        }
        else
        {
            model.LoadData(null);
        }

        Debug.Log($"[SaveSystem] Loaded: ICoordsModel");

        return;
    }

    public static async UniTask TryLoad(IAppearenceViewModel model)
    {
        if (MirraSDK.Data.HasKey(SaveKeyAppearence))
        {
            AppearenceSaveData data = MirraSDK.Data.GetObject<AppearenceSaveData>(SaveKeyAppearence);
            model.LoadData(data);
        }
        else
        {
            model.LoadData(null);
        }

        Debug.Log($"[SaveSystem] Loaded: IAppearenceModel");

        return;
    }

    public static async UniTask TryLoad(ISoundViewModel model)
    {
        if (MirraSDK.Data.HasKey(SaveKeySound))
        {
            SoundSaveData data = MirraSDK.Data.GetObject<SoundSaveData>(SaveKeySound);
            model.LoadData(data);
        }
        else
        {
            model.LoadData(null);
        }

        Debug.Log($"[SaveSystem] Loaded: ISoundModel");

        return;
    }

    public static async UniTask ResetSaves()
    {
        MirraSDK.Data.DeleteAll();
    }
}
