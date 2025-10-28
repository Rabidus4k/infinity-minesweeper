using UnityEngine;
using Zenject;

public class SaveControllerView : MonoBehaviour
{
    private ISaveService _saveService;

    [Inject]
    private void Construct(ISaveService saveService)
    {
        _saveService = saveService;
    }

    private void OnEnable()
    {
        Application.focusChanged += OnFocusChanged;
        Application.quitting += OnAppQuit;
    }

    private void OnDisable()
    {
        Application.focusChanged -= OnFocusChanged;
        Application.quitting -= OnAppQuit;
    }

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

    public void ResetProgress()
    {
        _saveService.ResetSaves();
    }
}
