using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameGridManagerView : MonoBehaviour
{
    private Dictionary<Vector3Int, List<Cell>> _grid = new Dictionary<Vector3Int, List<Cell>>();
    private Dictionary<Vector3Int, GridView> _gridViews = new Dictionary<Vector3Int, GridView>();
    private GridView.Factory _gridFactory;

    protected IInputViewModel _inputViewModel;
    protected IGameViewModel _gameViewModel;

    [Inject]
    private void Construct(IInputViewModel inputViewModel, IGameViewModel gameViewModel, GridView.Factory factory)
    {
        _inputViewModel = inputViewModel;
        _gameViewModel = gameViewModel;

        _gridFactory = factory;

        _gameViewModel.LastSelectCell.OnChanged += HandleSelectCell;
    }

    private void OnDisable()
    {
        _gameViewModel.LastSelectCell.OnChanged -= HandleSelectCell;
    }

    private void HandleSelectCell(Cell cell)
    {
        Vector3Int origin = GridHelper.ConvertToGridCoords(cell.Coords, _gameViewModel.Config.Value.Size);

        if (!_grid.ContainsKey(origin))
        {
            _grid.Add(origin, new List<Cell>());

            var gridView = _gridFactory.Create();
            gridView.transform.position = origin;
            gridView.Initialize(0, _gameViewModel.Config.Value.Size * _gameViewModel.Config.Value.Size);

            _gridViews.Add(origin, gridView);
        }

        if (cell.Info.IsOpened)
        {
            if (_grid[origin].Contains(cell)) return;

            if (cell.Info.Value == -1)
            {
                _gridViews[origin].SetState(GridStates.Death);
            }

            _grid[origin].Add(cell);
            _gridViews[origin].AddValue(1);
        }
        else 
        {
            if (cell.Info.IsFlagged && cell.Info.Value == -1)
            {
                if (_grid[origin].Contains(cell)) return;
                _grid[origin].Add(cell);

                _gridViews[origin].AddValue(1);
            }
            else if (!cell.Info.IsFlagged)
            {
                if (!_grid[origin].Contains(cell)) return;

                _grid[origin].Remove(cell);

                _gridViews[origin].AddValue(-1);
            }
        }
    }
}
