using UnityEngine;
using VInspector;
using Zenject;

public class GridView : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _valueText;

    [SerializeField] private SerializedDictionary<GridStates, UIPanel> _panels = new SerializedDictionary<GridStates, UIPanel>();

    public GridStates CurrentState = GridStates.None;

    private int _value = 0;
    private int _maxValue = 0;

    private UIPanel _currentPanel;

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
            if (CurrentState == GridStates.None)
                SetState(GridStates.Opened);
        }
    }

    public void Reviwe()
    {
        SetState(GridStates.Reviwe);
    }

    public void SetState(GridStates state)
    {
        if (CurrentState == state) return;

        CurrentState = state;

        ShowPanel(state);

        switch (CurrentState)
        {
            case GridStates.None:
                {
                    if (_value == _maxValue)
                        SetState(GridStates.Opened);
                    break;
                }
            case GridStates.Reviwe:
                {
                    SetState(GridStates.None);
                    break;
                }
        }
    }

    private void ShowPanel(GridStates state)
    {
        if (_currentPanel != null)
            _currentPanel.HidePanel(0.3f);

        if (!_panels.ContainsKey(state)) return;

        _currentPanel = _panels[state];
        _currentPanel.ShowPanel(0.3f);
    }

    public class Factory : PlaceholderFactory<GridView>
    {
    }
}