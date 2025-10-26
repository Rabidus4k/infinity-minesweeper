using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class CurrencySpenderView : MonoBehaviour
{
    [SerializeField] protected int _spendAmmount;
    
    protected ICurrencyViewModel _currencyViewModel;

    public UnityEvent OnEnoughtCurrency;
    public UnityEvent OnNotEnoughtCurrency;

    public UnityEvent OnSpend;

    private bool _isEnought = false;

    [Inject]
    private void Construct(ICurrencyViewModel currencyViewModel)
    {
        _currencyViewModel = currencyViewModel;

        _currencyViewModel.Gems.OnChanged += SetUpCurrencySpender;

        SetUpCurrencySpender(_currencyViewModel.Gems.Value);
    }

    private void OnDisable()
    {
        _currencyViewModel.Gems.OnChanged -= SetUpCurrencySpender;
    }

    private void SetUpCurrencySpender(int value)
    {
        if (value < _spendAmmount)
        {
            _isEnought = false;
            OnNotEnoughtCurrency?.Invoke();
        }
        else
        {
            _isEnought = true;
            OnEnoughtCurrency?.Invoke();
        }
    }

    public void SpendCurrency()
    {
        if (!_isEnought) return;

        _currencyViewModel.SpendGems(_spendAmmount);
        OnSpend?.Invoke();
    }
}
