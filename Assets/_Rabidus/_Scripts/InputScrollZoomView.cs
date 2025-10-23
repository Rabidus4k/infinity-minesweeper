using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InputScrollZoomView : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    protected IInputViewModel _viewModel;

    [Inject]
    protected void Construct(IInputViewModel viewModel)
    {
        _viewModel = viewModel;

        _slider.maxValue = _viewModel.ZoomConfig.Value.MaxZoom;
        _slider.minValue = _viewModel.ZoomConfig.Value.MinZoom;

        _viewModel.Zoom.OnChanged += UpdateSliderValue;

        UpdateSliderValue(_viewModel.Zoom.Value);
    }

    private void Awake()
    {
        _slider.onValueChanged.AddListener(HandleSliderValueChange);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(HandleSliderValueChange);
        _viewModel.Zoom.OnChanged -= UpdateSliderValue;
    }

    private void HandleSliderValueChange(float value)
    {
        _viewModel.HandleSliderZoom(value);
    }

    private void UpdateSliderValue(float value)
    {
        _slider.value = value;
    }
}
