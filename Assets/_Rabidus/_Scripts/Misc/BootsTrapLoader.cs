using Cysharp.Threading.Tasks;
using MirraGames.SDK;
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
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        SceneManager.LoadScene("GameScene");
        MirraSDK.Analytics.GameIsReady();
    }
}
