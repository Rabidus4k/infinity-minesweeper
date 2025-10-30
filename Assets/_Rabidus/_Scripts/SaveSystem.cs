using Cysharp.Threading.Tasks;
using MirraGames.SDK;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public static void Save(ICurrencyModel model)
    {
        CurrencySaveData data = new CurrencySaveData(model);
        //MirraSDK.Data.SetObject<CurrencySaveData>(SaveKeyCurrency, data, important: true);
        Debug.Log($"[SaveSystem] Save: ICurrencyModel");

        SaveObjectDataAsync(SaveKeyCurrency, data).Forget();
    }

    public static void Save(IScoreModel model)
    {
        ScoreSaveData data = new ScoreSaveData(model);
        //MirraSDK.Data.SetObject<ScoreSaveData>(SaveKeyScore, data, important: true);
        Debug.Log($"[SaveSystem] Save: IScoreModel");

        SaveObjectDataAsync(SaveKeyScore, data).Forget();
    }

    public static void Save(IGameModel model)
    {
        GameSaveData data = new GameSaveData(model);
        //MirraSDK.Data.SetObject<GameSaveData>(SaveKeyGame, data, important: true);
        Debug.Log($"[SaveSystem] Save: IGameModel");

        SaveObjectDataAsync(SaveKeyGame, data).Forget();
    }

    public static void Save(ICoordsModel model)
    {
        CoordsSaveData data = new CoordsSaveData(model);
        //MirraSDK.Data.SetObject<CoordsSaveData>(SaveKeyCoords, data, important: true);
        Debug.Log($"[SaveSystem] Save: ICoordsModel");

        SaveObjectDataAsync(SaveKeyCoords, data).Forget();
    }

    public static void Save(IAppearenceModel model)
    {
        AppearenceSaveData data = new AppearenceSaveData(model);
        //MirraSDK.Data.SetObject<AppearenceSaveData>(SaveKeyAppearence, data, important: true);
        Debug.Log($"[SaveSystem] Save: IAppearenceModel");

        SaveObjectDataAsync(SaveKeyAppearence, data).Forget();
    }

    public static void Save(ISoundModel model)
    {
        SoundSaveData data = new SoundSaveData(model);
        //MirraSDK.Data.SetObject<SoundSaveData>(SaveKeySound, data, important: true);
        Debug.Log($"[SaveSystem] Save: ISoundModel");

        SaveObjectDataAsync(SaveKeySound, data).Forget();
    }

    private static async UniTask SaveObjectDataAsync(string key, object data)
    {
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

    public static async UniTask TryLoad(ICurrencyModel model)
    {
        //if (!MirraSDK.Data.HasKey(SaveKeyCurrency)) return;

        //CurrencySaveData data = MirraSDK.Data.GetObject<CurrencySaveData>(SaveKeyCurrency);
        //Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        CurrencySaveData data = await RetrieveSpecificData<CurrencySaveData>(SaveKeyCurrency);
        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: ICurrencyModel");
    }

    public static async UniTask TryLoad(IScoreModel model)
    {
        //if (!MirraSDK.Data.HasKey(SaveKeyScore)) return;

        //ScoreSaveData data = MirraSDK.Data.GetObject<ScoreSaveData>(SaveKeyScore);
        //Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        ScoreSaveData data = await RetrieveSpecificData<ScoreSaveData>(SaveKeyScore);
        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: IScoreModel");
    }

    public static async UniTask TryLoad(IGameModel model)
    {
        //if (!MirraSDK.Data.HasKey(SaveKeyGame)) return;

        //GameSaveData data = MirraSDK.Data.GetObject<GameSaveData>(SaveKeyGame);
        //Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        GameSaveData data = await RetrieveSpecificData<GameSaveData>(SaveKeyGame);
        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: IGameModel");
    }

    public static async UniTask TryLoad(ICoordsModel model)
    {
        //if (!MirraSDK.Data.HasKey(SaveKeyCoords)) return;

        //CoordsSaveData data = MirraSDK.Data.GetObject<CoordsSaveData>(SaveKeyCoords);
        //Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        CoordsSaveData data = await RetrieveSpecificData<CoordsSaveData>(SaveKeyCoords);
        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: ICoordsModel");

        return;
    }

    public static async UniTask TryLoad(IAppearenceModel model)
    {
        //if (!MirraSDK.Data.HasKey(SaveKeyAppearence)) return;

        //AppearenceSaveData data = MirraSDK.Data.GetObject<AppearenceSaveData>(SaveKeyAppearence);
        //Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        AppearenceSaveData data = await RetrieveSpecificData<AppearenceSaveData>(SaveKeyAppearence);
        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: IAppearenceModel");

        return;
    }

    public static async UniTask TryLoad(ISoundModel model)
    {
        //if (!MirraSDK.Data.HasKey(SaveKeySound)) return;

        //SoundSaveData data = MirraSDK.Data.GetObject<SoundSaveData>(SaveKeySound);
        //Debug.Log(JsonUtility.ToJson(data, prettyPrint: true));

        SoundSaveData data = await RetrieveSpecificData<SoundSaveData>(SaveKeySound);
        if (data != null)
        {
            model.LoadData(data);
        }

        Debug.Log($"[SaveSystem] Loaded: ISoundModel");

        return;
    }

    private static async UniTask<T> RetrieveSpecificData<T>(string key)
    {
        try
        {
            var results = await CloudSaveService.Instance.Data.Player.LoadAsync(
                new HashSet<string> { key }
            );

            if (results.TryGetValue(key, out var item))
            {
                return item.Value.GetAs<T>();
            }
            else
            {
                Debug.Log($"There is no such key as {key}!");
            }
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

        return default;
    }

    public static async UniTask ResetSaves()
    {
        //MirraSDK.Data.DeleteAll();

        await ForceDeleteSpecificData(SaveKeyCurrency);
        await ForceDeleteSpecificData(SaveKeyScore);
        await ForceDeleteSpecificData(SaveKeyGame);
        await ForceDeleteSpecificData(SaveKeyCoords);
        await ForceDeleteSpecificData(SaveKeyAppearence);
        await ForceDeleteSpecificData(SaveKeySound);
    }

    private static async UniTask ForceDeleteSpecificData(string key)
    {
        try
        {
            // Deletion of the key without write lock validation by omitting the DeleteOptions argument
            await CloudSaveService.Instance.Data.Player.DeleteAllAsync();

            Debug.Log($"Successfully deleted {key}");
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
}
