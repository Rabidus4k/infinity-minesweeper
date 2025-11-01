using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SaveControllerView : MonoBehaviour
{
    private ISaveService _saveService;

    [Inject]
    private void Construct(ISaveService saveService)
    {
        _saveService = saveService;
    }

    public void SaveProgress()
    {
        _saveService.SaveCurrency();
        _saveService.SaveScore();
        _saveService.SaveGame();
        _saveService.SaveCoords();
        _saveService.SaveScore();
    }

    public void ResetProgress()
    {
        _saveService.ResetSaves();
        SceneManager.LoadScene("GameScene");
    }
}
