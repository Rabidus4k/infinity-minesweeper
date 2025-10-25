using UnityEngine;
using Zenject;

public class GridView : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _valueText;
    [SerializeField] private GameObject _fillBadget;

    private int _value = 0;
    private int _maxValue = 0;

    private IScoreViewModel _scoreViewModel;

    [Inject]
    private void Construct(IScoreViewModel scoreViewModel)
    {
        _scoreViewModel = scoreViewModel;
    }

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
            _scoreViewModel.AddScore(_maxValue);
        }
    }


    public class Factory : PlaceholderFactory<GridView>
    {
    }

}