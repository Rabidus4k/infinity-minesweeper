using UnityEngine;
using Zenject;

public class UIMusicButton : UICustomButton
{
    private ISoundViewModel _viewModel;

    [Inject]
    private void Construct(ISoundViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    protected override void HandleClick()
    {
        _viewModel.ToggleMusic();
    }
}
