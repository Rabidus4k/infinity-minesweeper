using System.Collections.Generic;
using System.Dynamic;
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

    protected IGameViewModel _gameViewModel;
    private SoundManager _soundManager;

    [Inject]
    private void Construct(IGameViewModel gameViewModel, SoundManager soundManager)
    {
        _soundManager = soundManager;
        _gameViewModel = gameViewModel;
    }

    public void Initialize(int maxValue)
    {
        _maxValue = maxValue;
    }

    public void AddValue(int ammount)
    {
        _value += ammount;
        _valueText.SetText(_value.ToString());

        if (_value == _maxValue && CurrentState == GridStates.None)
        {
            SetState(GridStates.Reward);
        }
    }

    public void Reviwe()
    {
        SetState(GridStates.Reviwe);
    }

    public void Opened()
    {
        SetState(GridStates.Opened);
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
                        SetState(GridStates.Reward);
                    break;
                }
            case GridStates.Reviwe:
                {
                    SetState(GridStates.None);
                    break;
                }
            case GridStates.Reward:
                {
                    _soundManager.PlaySound("GridVictory");
                    break;
                }
            case GridStates.Death:
                {
                    _soundManager.PlaySound("GridLose");
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