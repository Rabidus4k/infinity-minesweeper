using Cysharp.Threading.Tasks;
using MirraGames.SDK;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapLoader : MonoBehaviour
{
    private void Awake()
    {
        MirraSDK.WaitForProviders(() => {
            PrepareGame().Forget();
        });
    }

    private async UniTask PrepareGame()
    {
        try
        {
            await SceneManager.LoadSceneAsync("GameScene");
            MirraSDK.Analytics.GameIsReady();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError(ex);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
