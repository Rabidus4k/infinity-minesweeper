using System;
using UnityEngine;
using Zenject;

public class CurrencyView : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI _gemsText;

    protected ICurrencyViewModel _currencyViewModel;

    [Inject]
    private void Construct(ICurrencyViewModel currencyViewModel)
    {
        _currencyViewModel = currencyViewModel;
        _currencyViewModel.Gems.OnChanged += SetGems;

        SetGems(_currencyViewModel.Gems.Value);
    }

    protected void OnDisable()
    {
        _currencyViewModel.Gems.OnChanged -= SetGems;
    }

    private void SetGems(int value)
    {
        _gemsText.SetText(value.ToString());
    }
}
