using UnityEngine;
using VInspector;
using Zenject;

public class DebugView : MonoBehaviour
{
    private ISaveService _saveService;

    [Inject]
    private void Construct(ISaveService saveService)
    {
        _saveService = saveService;
    }

    [Button]
    private void SaveData()
    {
        _saveService.Save();
    }

    [Button]
    protected void DeleteData()
    {
        _saveService.ResetSaves();
    }
}
