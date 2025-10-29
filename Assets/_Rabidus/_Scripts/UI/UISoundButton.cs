using UnityEngine;
using Zenject;

public class UISoundButton : UICustomButton
{
    private SoundManager _soundManager;
    [Inject]
    private void Construct(SoundManager soundManager)
    {
        _soundManager = soundManager;
    }

    protected override void HandleClick()
    {
        _soundManager.ToggleSound();
    }
}
