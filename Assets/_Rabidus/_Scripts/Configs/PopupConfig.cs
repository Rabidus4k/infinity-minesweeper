using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Popup")]
public class PopupConfig : ScriptableObject, IPopupConfig
{
    [field: SerializeField] public float Delay { get; private set; }
    [field: SerializeField] public float Time { get; private set; }
}

public interface IPopupConfig
{
    public float Delay { get; }
    public float Time { get; }
}
