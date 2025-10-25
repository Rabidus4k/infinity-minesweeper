using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;
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
        Vector3Int origin = new Vector3Int
            (
                Mathf.FloorToInt((float)cell.Coords.x / _gameViewModel.Config.Value.Size) * _gameViewModel.Config.Value.Size,
                Mathf.FloorToInt((float)cell.Coords.y / _gameViewModel.Config.Value.Size) * _gameViewModel.Config.Value.Size,
                0
            );

        if (cell.Info.IsOpened)
        {
            if (_grid.ContainsKey(origin))
            {
                if (_grid[origin].Contains(cell)) return;
                _grid[origin].Add(cell);

                _gridViews[origin].AddValue(1);
            }
            else
            {
                _grid.Add(origin, new List<Cell>() { cell });

                //Instantiate(_gridView, origin, Quaternion.identity);

                var gridView = _gridFactory.Create();
                gridView.transform.position = origin;

                gridView.Initialize(1, _gameViewModel.Config.Value.Size * _gameViewModel.Config.Value.Size);
                _gridViews.Add(origin, gridView);
            }
        }
        else 
        {
            if (cell.Info.IsFlagged && cell.Info.Value == -1)
            {
                if (_grid.ContainsKey(origin))
                {
                    if (_grid[origin].Contains(cell)) return;
                    _grid[origin].Add(cell);

                    _gridViews[origin].AddValue(1);
                }
                else
                {
                    _grid.Add(origin, new List<Cell>() { cell });

                    //var gridView = Instantiate(_gridView, origin, Quaternion.identity);

                    var gridView = _gridFactory.Create();
                    gridView.transform.position = origin;

                    gridView.Initialize(1, _gameViewModel.Config.Value.Size * _gameViewModel.Config.Value.Size);
                    _gridViews.Add(origin, gridView);
                }
            }
            else if (!cell.Info.IsFlagged)
            {
                if (_grid.ContainsKey(origin))
                {
                    if (_grid[origin].Contains(cell))
                    {
                        _grid[origin].Remove(cell);

                        _gridViews[origin].AddValue(-1);
                    }
                }
            }
        }
    }
}
