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

    public void SaveProgress()
    {
        _saveService.Save();
    }

    public void ResetProgress()
    {
        _saveService.ResetSaves();
    }
}
