using Cysharp.Threading.Tasks;
using Lean.Pool;
using System;
using UnityEngine;
using VInspector;
using Zenject;

public class PopupView : MonoBehaviour
{
    [SerializeField] private RectTransform _target;
    [SerializeField] private float _radius;
    [SerializeField] private LeanGameObjectPool _pool;

    protected ICurrencyViewModel _currencyViewModel;
    protected IPopupService _popupService;

    private int _lastValue = 0;

    [Inject]
    private void Construct(ICurrencyViewModel currencyViewModel, IPopupService popupService)
    {
        _popupService = popupService;
        _currencyViewModel = currencyViewModel;
        _currencyViewModel.Gems.OnChanged += SpawnPopup;

        _lastValue = _currencyViewModel.Gems.Value;
    }

    private void OnDisable()
    {
        _currencyViewModel.Gems.OnChanged -= SpawnPopup;
    }

    private void SpawnPopup(int value)
    {
        var difference = value - _lastValue;
        _lastValue = value;

        if (difference > 0)
            SpwanPopupAsync(difference).Forget();
    }

    private async UniTask SpwanPopupAsync(int value)
    {
        value = Mathf.Clamp(value, 0, 10);

        for (int i = 0; i < value; i++)
        {
            var randomInCircle = UnityEngine.Random.insideUnitCircle * _radius;
            var popupInstance = _pool.Spawn(transform).GetComponent<Popup>();
            
            popupInstance.transform.localPosition = randomInCircle;
            popupInstance.Initialize(_target);

            await UniTask.WaitForEndOfFrame();
        }
    }
}
