using System;
using UnityEngine;
using Zenject;

public class CoordsView : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _coordsText;

    protected IInputViewModel _inputViewModel;

    [Inject]
    private void Construct(IInputViewModel inputViewModel)
    {
        _inputViewModel = inputViewModel;

        _inputViewModel.Coords.OnChanged += SetCoords;

        SetCoords(_inputViewModel.Coords.Value);
    }

    private void OnDisable()
    {
        _inputViewModel.Coords.OnChanged -= SetCoords;
    }

    private void SetCoords(Vector3Int coords)
    {
        _coordsText.SetText($"X:{coords.x}\nY:{coords.y}");
    }
}
