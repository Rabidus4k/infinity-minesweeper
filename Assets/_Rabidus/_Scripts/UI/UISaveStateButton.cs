using MirraGames.SDK;
using UnityEngine;

public class UISaveStateButton : UICustomButton
{
    [SerializeField] protected bool _doSave = true;
    [SerializeField] protected string _ID = string.Empty;

    [SerializeField] protected bool _state = true;

    protected override void Awake()
    {
        base.Awake();
        _state = MirraSDK.Data.GetBool(_ID);
    }

    protected override void HandleClick()
    {
        throw new System.NotImplementedException();
    }
}
