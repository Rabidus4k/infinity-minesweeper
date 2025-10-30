using System;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "Configs/Theme")]
public class ThemeConfig : ScriptableObject, IThemeConfig
{
    [field: SerializeField] public Color MainColor { get; private set; }
    [field: SerializeField] public Color SecondaryColor { get; private set; }
    [field: SerializeField] public Color AdditionalColor { get; private set; }
}

public interface IThemeConfig 
{
    public Color MainColor { get; }
    public Color SecondaryColor { get; }
    public Color AdditionalColor { get; }
}
