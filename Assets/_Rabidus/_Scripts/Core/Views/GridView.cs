using UnityEngine;
using Zenject;

public class GridView : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _valueText;
    [SerializeField] private GameObject _fillBadget;

    private int _value = 0;
    private int _maxValue = 0;

    public void Initialize(int value, int maxValue)
    {
        _maxValue = maxValue;
        AddValue(value);
    }

    public void AddValue(int ammount)
    {
        _value += ammount;
        _valueText.SetText(_value.ToString());

        if (_value == _maxValue) 
        {
            _fillBadget.SetActive(true);
        }
    }


    public class Factory : PlaceholderFactory<GridView>
    {
    }

}