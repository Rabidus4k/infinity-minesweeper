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

    public static async UniTask Save(ICurrencyModel model)
    {
        CurrencySaveData data = new CurrencySaveData(model);
        //MirraSDK.Data.SetObject<CurrencySaveData>(SaveKeyCurrency, data, important: true);
        Debug.Log($"[SaveSystem] Save: ICurrencyModel");

        await SaveObjectDataAsync(SaveKeyCurrency, data);
    }

    public static async UniTask Save(IScoreModel model)
    {
        ScoreSaveData data = new ScoreSaveData(model);
        //MirraSDK.Data.SetObject<ScoreSaveData>(SaveKeyScore, data, important: true);
        Debug.Log($"[SaveSystem] Save: IScoreModel");

        await SaveObjectDataAsync(SaveKeyScore, data);
    }

    public static async UniTask Save(IGameModel model)
    {
        GameSaveData data = new GameSaveData(model);
        //MirraSDK.Data.SetObject<GameSaveData>(SaveKeyGame, data, important: true);
        Debug.Log($"[SaveSystem] Save: IGameModel");

        await SaveObjectDataAsync(SaveKeyGame, data);
    }

    public static async UniTask Save(ICoordsModel model)
    {
        CoordsSaveData data = new CoordsSaveData(model);
        //MirraSDK.Data.SetObject<CoordsSaveData>(SaveKeyCoords, data, important: true);
        Debug.Log($"[SaveSystem] Save: ICoordsModel");

        await SaveObjectDataAsync(SaveKeyCoords, data);
    }

    public static async UniTask Save(IAppearenceModel model)
    {
        AppearenceSaveData data = new AppearenceSaveData(model);
        //MirraSDK.Data.SetObject<AppearenceSaveData>(SaveKeyAppearence, data, important: true);
        Debug.Log($"[SaveSystem] Save: IAppearenceModel");

        await SaveObjectDataAsync(SaveKeyAppearence, data);
    }

    public static async UniTask Save(ISoundModel model)
    {
        SoundSaveData data = new SoundSaveData(model);
        //MirraSDK.Data.SetObject<SoundSaveData>(SaveKeySound, data, important: true);
        Debug.Log($"[SaveSystem] Save: ISoundModel");

        await SaveObjectDataAsync(SaveKeySound, data);
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

    public static async UniTask TryLoad(ICurrencyViewModel model)
    {
        CurrencySaveData data = await RetrieveSpecificData<CurrencySaveData>(SaveKeyCurrency);
        model.LoadData(data);

        Debug.Log($"[SaveSystem] Loaded: ICurrencyModel");
    }

    public static async UniTask TryLoad(IScoreViewModel model)
    {
        ScoreSaveData data = await RetrieveSpecificData<ScoreSaveData>(SaveKeyScore);
        model.LoadData(data);

        Debug.Log($"[SaveSystem] Loaded: IScoreModel");
    }

    public static async UniTask TryLoad(IGameViewModel model)
    {
        GameSaveData data = await RetrieveSpecificData<GameSaveData>(SaveKeyGame);
        model.LoadData(data);

        Debug.Log($"[SaveSystem] Loaded: IGameModel");
    }

    public static async UniTask TryLoad(ICoordsViewModel model)
    {
        CoordsSaveData data = await RetrieveSpecificData<CoordsSaveData>(SaveKeyCoords);
        model.LoadData(data);

        Debug.Log($"[SaveSystem] Loaded: ICoordsModel");

        return;
    }

    public static async UniTask TryLoad(IAppearenceViewModel model)
    {
        AppearenceSaveData data = await RetrieveSpecificData<AppearenceSaveData>(SaveKeyAppearence);
        model.LoadData(data);

        Debug.Log($"[SaveSystem] Loaded: IAppearenceModel");

        return;
    }

    public static async UniTask TryLoad(ISoundViewModel model)
    {
        SoundSaveData data = await RetrieveSpecificData<SoundSaveData>(SaveKeySound);
        model.LoadData(data);

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
