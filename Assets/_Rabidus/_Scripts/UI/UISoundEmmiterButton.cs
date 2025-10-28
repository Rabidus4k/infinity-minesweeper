using UnityEngine;
using Zenject;

public class UISoundEmmiterButton : UICustomButton
{
    [SerializeField] private Sound _sound;

    private SoundManager _soundManager;

    protected override void HandleClick()
    {
        _soundManager.PlaySound(_sound);
    }

    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }
}
