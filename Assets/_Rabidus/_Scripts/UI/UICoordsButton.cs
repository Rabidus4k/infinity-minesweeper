using System;
using TMPro;
using UnityEngine;
using Zenject;

public class UICoordsButton : UICustomButton
{
    [SerializeField] private TMPro.TextMeshProUGUI _coordsText;
    [SerializeField] private TMPro.TMP_InputField _nameText;

    [SerializeField] private CoordsInfo _coordsInfo;

    private CoordsManagerView managerView;

    protected IInputViewModel _inputViewModel;
    private ICoordsViewModel _coordsViewModel;

    [Inject]
    private void Construct
    (
        IInputViewModel inputViewModel,
        ICoordsViewModel coordsViewModel
    )
    {
        _coordsViewModel = coordsViewModel;
        _inputViewModel = inputViewModel;
    }

    public void Initialize(CoordsManagerView coordsManagerView, CoordsInfo coordsInfo)
    {
        managerView = coordsManagerView;
        _coordsInfo = coordsInfo;

        RefreshInfo();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _nameText.onValueChanged.AddListener(OnValueChanged);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _nameText.onValueChanged.RemoveListener(OnValueChanged);
    }

    protected override void HandleClick()
    {
        _inputViewModel.HandleCameraCoords(_coordsInfo.Coords);
    }

    private void OnValueChanged(string _)
    {
        _coordsInfo.Name = _nameText.text;
    }

    private void RefreshInfo()
    {
        _nameText.text = _coordsInfo.Name;
        _coordsText.text = $"X:{_coordsInfo.Coords.x} Y:{_coordsInfo.Coords.y}";
    }

    public void DeleteButton()
    {
        Destroy(gameObject);
        managerView.DeleteButton(_coordsInfo);
    }

    public class Factory : PlaceholderFactory<UICoordsButton>
    {
    }
}
