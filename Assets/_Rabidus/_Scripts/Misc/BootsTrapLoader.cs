using MirraGames.SDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrapLoader : MonoBehaviour
{
    private void Awake()
    {
        MirraSDK.WaitForProviders(static () => {
            SceneManager.LoadScene("GameScene");
            MirraSDK.Analytics.GameIsReady();
        });
    }
}
