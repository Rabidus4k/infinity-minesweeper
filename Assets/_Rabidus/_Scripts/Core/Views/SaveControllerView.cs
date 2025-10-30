using Cysharp.Threading.Tasks;
using MirraGames.SDK;
using UnityEngine;
using Zenject;

public class SaveControllerView : MonoBehaviour
{
    [SerializeField] private float _saveInterval = 5f;

    private ISaveService _saveService;

    [Inject]
    private void Construct(ISaveService saveService)
    {
        _saveService = saveService;
    }

    private async void Start()
    {
        await _saveService.Load();
        SaveProgressAsync().Forget();
    }

    //private void OnEnable()
    //{
    //    Application.focusChanged += OnFocusChanged;
    //    Application.quitting += OnAppQuit;
    //}

    //private void OnDisable()
    //{
    //    Application.focusChanged -= OnFocusChanged;
    //    Application.quitting -= OnAppQuit;
    //}

    private void OnFocusChanged(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveProgress();
        }
    }

    private void OnAppQuit()
    {
        SaveProgress();
    }

    public void SaveProgress()
    {
        _saveService.SaveCurrency();
        _saveService.SaveScore();
        _saveService.SaveGame();
        _saveService.SaveCoords();
    }

    private async UniTask SaveProgressAsync()
    {
        while (true)
        {
            await UniTask.WaitForSeconds(_saveInterval);
            SaveProgress();
        }
    }

    public void ResetProgress()
    {
        _saveService.ResetSaves();
    }
}
