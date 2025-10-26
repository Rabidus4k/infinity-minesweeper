using MirraGames.SDK;
using UnityEngine;
using UnityEngine.Events;

public class UIRewardedAdsButton : UICustomButton
{
    [SerializeField] private string _rewardID;

    public UnityEvent OnSuccess;

    protected override void HandleClick()
    {
        MirraSDK.Ads.InvokeRewarded(
            onOpen: () => Debug.Log("������� �� �������������� �������"),
            onClose: (isSuccess) =>
            {
                Debug.Log($"������� �� �������������� ������� � �������� '{isSuccess}'");

                if (isSuccess)
                    OnSuccess?.Invoke();
            },
            rewardTag: _rewardID
        );
    }
}
