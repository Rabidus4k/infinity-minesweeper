using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CoordsManagerView : MonoBehaviour
{
    [SerializeField] private RectTransform _root;

    private UICoordsButton.Factory _factory;
    private IInputViewModel _inputViewModel;
    private ICoordsViewModel _coordsViewModel;

    [Inject]
    private void Construct
    (
        IInputViewModel inputViewModel,
        ICoordsViewModel coordsViewModel,
        UICoordsButton.Factory factory
    )
    {
        _coordsViewModel = coordsViewModel;
        _inputViewModel = inputViewModel;
        _factory = factory;

        foreach(var item in _coordsViewModel.CoordsInfo.Value)
        {
            CoordsButtonSetup(item);
        }
    }

    private void CoordsButtonSetup(CoordsInfo coordsInfo)
    {
        var button = _factory.Create();

        button.Initialize(this, coordsInfo);

        button.transform.SetParent(_root);
        button.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        button.transform.localScale = Vector3.one;

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_root);
    }

    public void CreateButton()
    {
        CoordsInfo coordsInfo = new CoordsInfo();
        coordsInfo.Name = "Location";
        coordsInfo.Coords = new Vector3Int
        (
            _inputViewModel.Coords.Value.x,
            _inputViewModel.Coords.Value.y,
            0
        );

        CoordsButtonSetup(coordsInfo);

        _coordsViewModel.CoordsInfo.Value.Add(coordsInfo);
    }

    public void DeleteButton(CoordsInfo coordsInfo)
    {
        _coordsViewModel.CoordsInfo.Value.Remove(coordsInfo);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_root);
    }
}
