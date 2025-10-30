using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class UITextThemeSetter : AppearenceThemeSetterView
{
    private TMPro.TextMeshProUGUI _text;

    protected override void Initialize()
    {
        _text = GetComponent<TMPro.TextMeshProUGUI>();  
    }

    protected override void ApplyTheme(ThemeConfig theme)
    {
        switch (_index)
        {
            case 0:
                {
                    _text.color = theme.MainColor;
                    break;
                }
            case 1:
                {
                    _text.color = theme.SecondaryColor;
                    break;
                }
            case 2:
                {
                    _text.color = theme.AdditionalColor;
                    break;
                }
        }
    }
}
