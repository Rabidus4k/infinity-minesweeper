using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;
using static UnityEngine.UI.Image;

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

            if (cell.Value == -1)
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
                    _addedCells.Remove(coords);

                AddValue(-1);
            }
        }
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