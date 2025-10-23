using UnityEngine;

public class InputViewModel : IInputViewModel
{
    public ReactiveProperty<float> Zoom { get; private set; } = new ReactiveProperty<float>();
    public ReactiveProperty<IZoomConfig> ZoomConfig { get; private set; } = new ReactiveProperty<IZoomConfig>();
    public ReactiveProperty<Vector2> DragDelta { get; private set; } = new ReactiveProperty<Vector2>();
    public ReactiveProperty<Vector3Int> LMBCoords { get; private set; } = new ReactiveProperty<Vector3Int>();
    public ReactiveProperty<Vector3Int> RMBCoords { get; private set; } = new ReactiveProperty<Vector3Int>();

    protected IInputModel _model;

    public InputViewModel(IInputModel model)
    {
        _model = model;

        Zoom.Value = _model.Zoom;
        ZoomConfig.Value = _model.ZoomConfig;
    }

    public void HandleZoom(float zoomInput)
    {
        if (Mathf.Approximately(zoomInput, 0f)) return;

        _model.HandleZoom(zoomInput);
        Zoom.Value = _model.Zoom;
    }

    public void HandleSliderZoom(float value)
    {
        if (Mathf.Approximately(value, _model.Zoom)) return;

        _model.HandleSliderZoom(value);
        Zoom.Value = _model.Zoom;
    }

    public void BeginDrag(Vector2 mousePosition)
    {
        _model.BeginDrag(mousePosition);
    }

    public void HandleDrag(Vector2 mousePosition)
    {
        _model.HandleDrag(mousePosition);
        DragDelta.Value = _model.Delta;
    }

    public void EndDrag()
    {
        _model.EndDrag();
    }

    public void HandleLeftMouseButtonClickCoords(Vector3Int coords)
    {
        _model.HandleLeftMouseButtonClick(coords);
        LMBCoords.Value = _model.LeftMouseButtonClick;
    }

    public void HandleRightMouseButtonClickCoords(Vector3Int coords)
    {
        _model.HandleRightMouseButtonClick(coords);
        RMBCoords.Value = _model.RightMouseButtonClick;
    }
}

public interface IInputViewModel
{
    public ReactiveProperty<float> Zoom { get; }
    public ReactiveProperty<IZoomConfig> ZoomConfig { get; }
    public ReactiveProperty<Vector2> DragDelta { get; }
    public ReactiveProperty<Vector3Int> LMBCoords { get; }
    public ReactiveProperty<Vector3Int> RMBCoords { get; }
    void HandleZoom(float zoom);
    void HandleSliderZoom(float value);
    void BeginDrag(Vector2 mousePosition);
    void HandleDrag(Vector2 mousePosition);
    void EndDrag();
    void HandleLeftMouseButtonClickCoords(Vector3Int coords);
    void HandleRightMouseButtonClickCoords(Vector3Int coords);
}
