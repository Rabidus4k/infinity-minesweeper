using System.Collections.Generic;
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

    private Dictionary<Vector3Int, CellInfo> _addedCells = new Dictionary<Vector3Int, CellInfo>();

    private UIPanel _currentPanel;

    protected IGameViewModel _gameViewModel;

    [Inject]
    private void Construct(IGameViewModel gameViewModel)
    {
        _gameViewModel = gameViewModel;
    }

    public void Initialize(int maxValue)
    {
        _maxValue = maxValue;
    }

    public void RefreshCell(Vector3Int coords)
    {
        if (!_gameViewModel.Cells.Value.ContainsKey(coords)) return;

        var cell = _gameViewModel.Cells.Value[coords];

        if (cell.IsOpened)
        {
            if (_addedCells.ContainsKey(coords)) return;
            _addedCells.Add(coords, cell);

            if (cell.Value == -1 && cell.Ignore == false)
                SetState(GridStates.Death);

            AddValue(1);
        }
        else
        {
            if (cell.IsFlagged && cell.Value == -1)
            {
                if (_addedCells.ContainsKey(coords)) return;
                _addedCells.Add(coords, cell);

                AddValue(1);
            }
            else if (!cell.IsFlagged)
            {
                if (_addedCells.ContainsKey(coords))
                {
                    _addedCells.Remove(coords);
                    AddValue(-1);
                }
            }
        }


        if (_value == _maxValue && CurrentState == GridStates.None)
        {
            if (cell.Ignore == false)
                SetState(GridStates.Reward);
            else
                SetState(GridStates.Opened);
        }
    }

    public void AddValue(int ammount)
    {
        _value += ammount;
        _valueText.SetText(_value.ToString());
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
                    foreach (var cell in _addedCells)
                    {
                        if (cell.Value.IsOpened && cell.Value.Value == -1)
                        {
                            _gameViewModel.Cells.Value[cell.Key] = new CellInfo()
                            {
                                Value = cell.Value.Value,
                                IsFlagged = cell.Value.IsFlagged,
                                IsOpened = cell.Value.IsOpened,
                                Ignore = true
                            };
                        }
                    }
                    SetState(GridStates.None);
                    break;
                }
            case GridStates.Reward:
                {
                    foreach (var cell in _addedCells)
                    {
                        _gameViewModel.Cells.Value[cell.Key] = new CellInfo()
                        {
                            Value = cell.Value.Value,
                            IsFlagged = cell.Value.IsFlagged,
                            IsOpened = cell.Value.IsOpened,
                            Ignore = true
                        };
                    }
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