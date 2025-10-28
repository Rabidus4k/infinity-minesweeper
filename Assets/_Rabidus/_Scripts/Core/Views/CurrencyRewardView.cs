using UnityEngine;
using Zenject;

public class CurrencyRewardView : MonoBehaviour
{
    [SerializeField] protected int _rewardAmmount;
    [SerializeField] private TMPro.TextMeshProUGUI _rewardText;

    protected ICurrencyViewModel _currencyViewModel;
    protected SoundManager _soundManager;
    [Inject]
    private void Construct(ICurrencyViewModel currencyViewModel, SoundManager soundManager)
    {
        _soundManager = soundManager;
        _currencyViewModel = currencyViewModel;
    }

    private void Awake()
    {
        if (_rewardText != null)
        {
            _rewardText.SetText(_rewardAmmount.ToString());
        }
    }

    public void ClaimReward()
    {
        _soundManager.PlaySound("Coin");
        _currencyViewModel.AddGems(_rewardAmmount);
    }

    public void ClaimDoubleReward()
    {
        _soundManager.PlaySound("Coin");
        _currencyViewModel.AddGems(_rewardAmmount * 2);
    }
}
