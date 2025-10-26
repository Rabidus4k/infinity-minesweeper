using UnityEngine;
using Zenject;

public class UICoordsButton : UICustomButton
{
    [SerializeField] private Vector3 _coords;

    protected IInputViewModel _inputViewModel;

    [Inject]
    private void Construct(IInputViewModel inputViewModel)
    {
        _inputViewModel = inputViewModel;
    }

    public void Initialize(Vector3 coords)
    {
        _coords = coords;
    }

    protected override void HandleClick()
    {
        _inputViewModel.HandleCameraCoords(_coords);
    }

    public class Factory : PlaceholderFactory<UICoordsButton>
    {
    }
}
