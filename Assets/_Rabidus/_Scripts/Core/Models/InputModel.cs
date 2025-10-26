using UnityEngine;

public class InputModel : IInputModel
{
    public IZoomConfig ZoomConfig { get; private set; }
    public float Zoom { get; private set; }
    public bool IsDragging { get; private set; }
    public Vector2 Delta { get ; private set; }
    public Vector2 LastMousePos { get; private set; }
    public Vector3Int LeftMouseButtonClick { get; private set; }
    public Vector3Int RightMouseButtonClick { get; private set; }
    public Vector3Int Coords { get; private set; }
    public Vector3 CameraCoords { get; private set; }

    public InputModel(IZoomConfig zoomConfig)
    {
        ZoomConfig = zoomConfig;
    }

    public void HandleZoom(float zoomInput)
    {
        Zoom = Mathf.Clamp(
            Zoom - zoomInput * ZoomConfig.ZoomStep,
            ZoomConfig.MinZoom, ZoomConfig.MaxZoom
        );
    }

    public void HandleSliderZoom(float value)
    {
        Zoom = Mathf.Clamp(value, ZoomConfig.MinZoom, ZoomConfig.MaxZoom);
    }

    public void BeginDrag(Vector2 mousePosition)
    {
        IsDragging = true;
        LastMousePos = mousePosition;
    }

    public void HandleDrag(Vector2 mousePosition)
    {
        Delta = mousePosition - LastMousePos;
        LastMousePos = mousePosition;
    }

    public void EndDrag()
    {
        IsDragging = false;
    }

    public void HandleLeftMouseButtonClick(Vector3Int coords)
    {
        LeftMouseButtonClick = coords;
    }

    public void HandleRightMouseButtonClick(Vector3Int coords)
    {
        RightMouseButtonClick = coords;
    }

    public void HandleCoords(Vector3Int coords)
    {
        Coords = coords;
    }

    public void HandleCameraCoords(Vector3 coords)
    {
        CameraCoords = coords;
    }
}

public interface IInputModel 
{
    public IZoomConfig ZoomConfig { get; }
    public float Zoom { get; }
    public bool IsDragging { get; }
    public Vector2 Delta { get; }
    public Vector2 LastMousePos { get; }
    public Vector3Int LeftMouseButtonClick { get; }
    public Vector3Int RightMouseButtonClick { get; }
    public Vector3Int Coords { get; }
    public Vector3 CameraCoords { get; }

    void HandleZoom(float zoomInput);
    void HandleSliderZoom(float value);
    void BeginDrag(Vector2 mousePosition);
    void HandleDrag(Vector2 mousePosition);
    void EndDrag();
    void HandleLeftMouseButtonClick(Vector3Int coords);
    void HandleRightMouseButtonClick(Vector3Int coords);
    void HandleCoords(Vector3Int coords);
    void HandleCameraCoords(Vector3 coords);
}
