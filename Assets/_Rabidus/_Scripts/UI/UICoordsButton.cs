using System;
using TMPro;
using UnityEngine;
using Zenject;

public class UICoordsButton : UICustomButton
{
    [SerializeField] private TMPro.TMP_InputField _xText;
    [SerializeField] private TMPro.TMP_InputField _yText;
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
        _xText.onValueChanged.AddListener(OnValueChanged);
        _yText.onValueChanged.AddListener(OnValueChanged);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _nameText.onValueChanged.RemoveListener(OnValueChanged);
        _xText.onValueChanged.RemoveListener(OnValueChanged);
        _yText.onValueChanged.RemoveListener(OnValueChanged);
    }

    protected override void HandleClick()
    {
        _inputViewModel.HandleCameraCoords(_coordsInfo.Coords);
    }

    private void OnValueChanged(string _)
    {
        if (int.TryParse(_xText.text, out int x))
            _coordsInfo.Coords.x = Mathf.Clamp(x, -100000, 100000);
        if (int.TryParse(_yText.text, out int y))
            _coordsInfo.Coords.y = Mathf.Clamp(y, -100000, 100000);

        _coordsInfo.Name = _nameText.text;
    }

    private void RefreshInfo()
    {
        _nameText.text = _coordsInfo.Name;
        _xText.text = _coordsInfo.Coords.x.ToString();
        _yText.text = _coordsInfo.Coords.y.ToString();
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
