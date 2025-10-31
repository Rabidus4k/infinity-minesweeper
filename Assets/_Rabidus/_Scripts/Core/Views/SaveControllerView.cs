using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class SaveControllerView : MonoBehaviour
{
    [SerializeField] private float _saveInterval = 5f;
    private CancellationTokenSource _cts;

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        CancelAndDispose();
    }

    private void OnDestroy()
    {
        CancelAndDispose();
    }

    private void CancelAndDispose()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    private ISaveService _saveService;

    [Inject]
    private void Construct(ISaveService saveService)
    {
        _saveService = saveService;
    }

    private async void Start()
    {
        await _saveService.Load();
        SaveProgressAsync(_cts.Token).Forget();
    }

    private async UniTask SaveProgressAsync(CancellationToken token)
    {
        while (true)
        {
            await _saveService.SaveCurrency();
            await _saveService.SaveScore();
            await _saveService.SaveGame();
            await _saveService.SaveCoords();
            await _saveService.SaveScore();
            await UniTask.WaitForSeconds(_saveInterval, cancellationToken: token);
        }
    }

    public void ResetProgress()
    {
        _saveService.ResetSaves();
    }
}
