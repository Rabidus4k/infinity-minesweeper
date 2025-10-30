using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageThemeSetter : AppearenceThemeSetterView
{
    protected Image _image;

    protected override void Initialize()
    {
        _image = GetComponent<Image>();
    }

    protected override void ApplyTheme(ThemeConfig theme)
    {
        switch (_index)
        {
            case 0:
                {
                    _image.color = theme.MainColor;
                    break;
                }
            case 1:
                {
                    _image.color = theme.SecondaryColor;
                    break;
                }
            case 2:
                {
                    _image.color = theme.AdditionalColor;
                    break;
                }
        }
    }
}

