using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class InputCameraControllerView : MonoBehaviour
{
    [SerializeField] private Camera cam;
    protected IInputViewModel _viewModel;

    private float _desiredZoom = 0;
    private bool _isManualMove = false;

    [Inject]
    private void Construct(IInputViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.Zoom.OnChanged += ApplyZoom;
        _viewModel.DragDelta.OnChanged += DragScreen;
        _viewModel.CameraCoords.OnChanged += ManualCameraMove;

        _viewModel.HandleZoom(cam.orthographicSize);
    }

    private void OnDisable()
    {
        _viewModel.Zoom.OnChanged -= ApplyZoom;
        _viewModel.DragDelta.OnChanged -= DragScreen;
        _viewModel.CameraCoords.OnChanged -= ManualCameraMove;
    }


    private void LateUpdate()
    {
        if (_isManualMove) return;

        if (!Mathf.Approximately(cam.orthographicSize, _desiredZoom))
        {
            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize, _desiredZoom, _viewModel.ZoomConfig.Value.ZoomLerpSpeed * Time.unscaledDeltaTime
            );
        }
    }

    public void DragScreen(Vector2 delta)
    {
        if (_isManualMove) return;

        float unitsPerPixelY = (cam.orthographicSize * 2f) / cam.pixelHeight;
        float unitsPerPixelX = (cam.orthographicSize * 2f * cam.aspect) / cam.pixelWidth;

        Vector3 worldDelta = new Vector3(
            -delta.x * unitsPerPixelX,
            -delta.y * unitsPerPixelY,
            0f
        );

        cam.transform.position += worldDelta;
    }

    public void ApplyZoom(float zoom)
    {
        _desiredZoom = zoom;
    }

    private void ManualCameraMove(Vector3 coords)
    {
        if (_isManualMove) return;

        CameraMoveAsync(coords).Forget();
    }

    private async UniTask CameraMoveAsync(Vector3 coords)
    {
        _isManualMove = true;

        cam.transform.DOMove(coords, 0.5f);
        await UniTask.WaitForSeconds(0.5f);

        _isManualMove = false;
    }
}
