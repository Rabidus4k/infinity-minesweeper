using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Zoom")]
public class ZoomConfig : ScriptableObject, IZoomConfig
{
    [field: SerializeField] public float MinZoom { get; private set; }
    [field: SerializeField] public float MaxZoom { get; private set; }
    [field: SerializeField] public float ZoomStep { get; private set; }
    [field: SerializeField] public bool SmoothZoom { get; private set; }
    [field: SerializeField] public float ZoomLerpSpeed { get; private set; }
}

public interface IZoomConfig
{
    public float MinZoom { get; }
    public float MaxZoom { get; }
    public float ZoomStep { get; }
    public bool SmoothZoom { get; }
    public float ZoomLerpSpeed { get; }
}