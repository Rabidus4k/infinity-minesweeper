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
            await UnityServices.InitializeAsync();


            if (MirraSDK.Player.UniqueId == null)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("[AuthenticationService] SignInAnonymouslyAsync");
            }
            else
            {
#if UNITY_EDITOR
                string uniqueId = "rabidus4k";
#else
                string uniqueId = MirraSDK.Player.UniqueId;
#endif
                string passCode = "12345678Aa!";

                try
                {
                    await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(uniqueId, passCode);
                    Debug.Log("[AuthenticationService] SignInWithUsernamePasswordAsync");
                }
                catch (AuthenticationException signInEx)
                {
                    try
                    {
                        await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(uniqueId, passCode);
                        Debug.Log("[AuthenticationService] SignUpWithUsernamePasswordAsync");
                    }
                    catch (RequestFailedException signUpEx)
                    {
                        throw new Exception(
                            $"Failed to sign in (and sign up fallback failed). SignIn error: [{signInEx.ErrorCode}] {signInEx.Message}; SignUp error: [{signUpEx.ErrorCode}] {signUpEx.Message}",
                            signInEx);
                    }
                }
            }

            SceneManager.LoadScene("GameScene");
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
