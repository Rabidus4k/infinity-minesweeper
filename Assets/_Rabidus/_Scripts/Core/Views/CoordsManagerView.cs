using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CoordsManagerView : MonoBehaviour
{
    [SerializeField] private RectTransform _root;

    private Camera _cam;

    private UICoordsButton.Factory _factory;
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
        _factory = factory;

        foreach(var item in _coordsViewModel.CoordsInfo.Value)
        {
            CoordsButtonSetup(item);
        }
    }

    private void Awake()
    {
        _cam = Camera.main;
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
        coordsInfo.Name = $"Location {_coordsViewModel.CoordsInfo.Value.Count}";
        coordsInfo.Coords = new Vector3Int
        (
            Mathf.FloorToInt(_cam.transform.position.x),
            Mathf.FloorToInt(_cam.transform.position.y),
            -10
        );

        CoordsButtonSetup(coordsInfo);

        _coordsViewModel.AddCoordsInfo(coordsInfo);
    }

    public void DeleteButton(CoordsInfo coordsInfo)
    {
        _coordsViewModel.RemoveCoordsInfo(coordsInfo);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_root);
    }
}
