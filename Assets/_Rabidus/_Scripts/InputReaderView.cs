using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputReaderView : MonoBehaviour
{
    protected IInputViewModel _viewModel;
    private Camera _cam;

    [Inject]
    private void Construct(IInputViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        HandleZoom();
        HandleDrag();
        HandleClick();
        
    }

    private void HandleClick()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (TryMouseToCell(out var coords))
                _viewModel.HandleLeftMouseButtonClickCoords(coords);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (TryMouseToCell(out var coords))
                _viewModel.HandleRightMouseButtonClickCoords(coords);
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _viewModel.BeginDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButton(2))
        {
            _viewModel.HandleDrag(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(2))
        {
            _viewModel.EndDrag();
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.0001f)
            _viewModel.HandleZoom(scroll);
    }

    private Vector3 MouseWorld()
    {
        var mp = Input.mousePosition;
        mp.z = Mathf.Abs(_cam.transform.position.z);
        return _cam.ScreenToWorldPoint(mp);
    }

    private bool TryMouseToCell(out Vector3Int coords)
    {
        var pos = MouseWorld();
        coords = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0);
        return true;
    }
}
