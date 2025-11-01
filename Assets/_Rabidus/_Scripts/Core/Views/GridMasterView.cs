using Cysharp.Threading.Tasks;
using Lean.Pool;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class FlagCellInfo 
{
    public CellView View;
    public bool IsFlagged;
}

public class GridMasterView : MonoBehaviour
{
    [SerializeField] private LeanGameObjectPool _cellPool;

    private readonly List<Vector3Int> _neighbours = new List<Vector3Int>()
    {
        Vector3Int.left,
        Vector3Int.right,

        Vector3Int.up,
        Vector3Int.down,

        Vector3Int.left + Vector3Int.up,
        Vector3Int.left + Vector3Int.down,
        Vector3Int.right + Vector3Int.up,
        Vector3Int.right + Vector3Int.down
    };

    private List<Vector3Int> _mineList = new List<Vector3Int>();
    private List<Vector3Int> _openedCells = new List<Vector3Int>();
    private List<Vector3Int> _gridCoords = new List<Vector3Int>();

    private Dictionary<Vector3Int, FlagCellInfo> _flaggedCells = new Dictionary<Vector3Int, FlagCellInfo>();
    private Dictionary<Vector3Int, GridView> _spawnedGrids = new Dictionary<Vector3Int, GridView>();

    private IGameViewModel _gameViewModel;
    private IInputViewModel _inputViewModel;
    private IScoreViewModel _scoreViewModel;

    private IGameConfig _config;
    private GridView.Factory _gridFactory;
    private CancellationTokenSource _cts;

    [Inject]
    private void Construct
    (
        IGameViewModel gameViewModel,
        IInputViewModel inputViewModel,
        IScoreViewModel scoreViewModel,
        GridView.Factory factory
    )
    {
        _cts = new CancellationTokenSource();

        _gridFactory = factory;

        _gameViewModel = gameViewModel;
        _inputViewModel = inputViewModel;
        _scoreViewModel = scoreViewModel;

        _inputViewModel.LMBCoords.OnChanged += HandleLeftClick;
        _inputViewModel.RMBCoords.OnChanged += HandleRightClick;
        _gameViewModel.IsLoaded.OnChanged += TryLoadGame;

        _config = _gameViewModel.Config.Value;

        if (_config.Seed != 0)
            Random.InitState(_config.Seed);

        TryLoadGame(_gameViewModel.IsLoaded.Value);
    }

    private void OnDisable()
    {
        _inputViewModel.LMBCoords.OnChanged -= HandleLeftClick;
        _inputViewModel.RMBCoords.OnChanged -= HandleRightClick;
        _gameViewModel.IsLoaded.OnChanged -= TryLoadGame;

        CancelAndDispose();
    }

    private void OnDestroy()
    {
        CancelAndDispose();
    }

    private void CancelAndDispose()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    private void TryLoadGame(bool value)
    {
        if (!value) return;

        if (_gameViewModel.OpenedCells.Value.Count > 0)
        {
            LoadGamefieldAsync(_cts.Token).Forget();
        }
    }

    private async UniTask LoadGamefieldAsync(CancellationToken token)
    {
        foreach (var coords in _gameViewModel.OpenedCells.Value)
        {
            var gridOrigin = GridHelper.ConvertToGridCoords(coords, _config.Size);

            if (!_gridCoords.Contains(gridOrigin))
            {
                await GenerateGridMines(gridOrigin, token);
            }

            int minesAround = 0;

            if (_mineList.Contains(coords))
            {
                minesAround = -1;
            }
            else
            {
                foreach (var neighbour in _neighbours)
                {
                    if (_mineList.Contains(coords + neighbour) && !_openedCells.Contains(coords + neighbour))
                        minesAround++;
                }
            }

            OpenTile(coords, minesAround);

            await UniTask.Yield(token);
        }

        _openedCells = _gameViewModel.OpenedCells.Value;
    }

    private void HandleLeftClick(Vector3Int coords)
    {
        if (_gameViewModel.IsLoaded.Value ==  false) return;
        HandleLeftClickAsync(coords, new List<Vector3Int>() , _cts.Token).Forget();
    }

    private void HandleRightClick(Vector3Int coords)
    {
        if (_gameViewModel.IsLoaded.Value == false) return;
        HandleRightClickAsync(coords, _cts.Token).Forget();
    }

    private async UniTask HandleLeftClickAsync(Vector3Int coords, List<Vector3Int> clickBuffer, CancellationToken token)
    {
        Debug.Log("[HandleLeftClickAsync]");

        var gridOrigin = GridHelper.ConvertToGridCoords(coords, _config.Size);

        if (_gridCoords.Contains(gridOrigin))
        {
            if (clickBuffer != null)
            {
                if (clickBuffer.Contains(coords)) return;
                clickBuffer.Add(coords);
            }

            if (_flaggedCells.ContainsKey(coords) && _flaggedCells[coords].IsFlagged) return;

            int trueMinesAround = 0;
            int minesAround = 0;
            int flagsAround = 0;

            if (_mineList.Contains(coords))
            {
                trueMinesAround = -1;
            }
            else
            {
                foreach (var neighbour in _neighbours)
                {
                    if (_mineList.Contains(coords + neighbour))
                    {
                        trueMinesAround++;
                        
                        if (!_openedCells.Contains(coords + neighbour))
                            minesAround++;
                    }   

                    if (_flaggedCells.ContainsKey(coords + neighbour) && _flaggedCells[coords + neighbour].IsFlagged)
                        flagsAround++;
                }
            }

            OpenTile(coords, trueMinesAround);

            await UniTask.Yield(token);

            if (trueMinesAround == -1)
                return;

            if (minesAround == 0 || flagsAround == minesAround)
            {
                foreach (var neighbour in _neighbours)
                {
                    if (!_openedCells.Contains(coords + neighbour))
                    {
                        await HandleLeftClickAsync(coords + neighbour, clickBuffer, token);
                    }
                }
            }
        }
        else
        {
            await GenerateGridMines(gridOrigin, token);
            await HandleLeftClickAsync(coords, clickBuffer, token);
        }
    }

    private async UniTask HandleRightClickAsync(Vector3Int coords, CancellationToken token)
    {
        Debug.Log("[HandleRightClickAsync]");

        var gridOrigin = GridHelper.ConvertToGridCoords(coords, _config.Size);

        if (_gridCoords.Contains(gridOrigin))
        {
            if (_openedCells.Contains(coords)) return;

            FlagTile(coords);
        }
        else
        {
            await GenerateGridMines(gridOrigin, token);
            await HandleRightClickAsync(coords, token);
        }
    }

    private void OpenTile(Vector3Int coords, int value)
    {
        if (_openedCells.Contains(coords)) return;

        _openedCells.Add(coords);

        var cellInstance = _cellPool.Spawn(coords, Quaternion.identity).GetComponent<CellView>();
        cellInstance.Initialize(_config.Sprites[value]);

        _scoreViewModel.AddScore(1);

        UpdateGridInfo(coords);
    }

    private void FlagTile(Vector3Int coords)
    {
        if (_flaggedCells.ContainsKey(coords))
        {
            _flaggedCells[coords].IsFlagged = !_flaggedCells[coords].IsFlagged;
        }
        else
        {
            var cellInstance = _cellPool.Spawn(coords, Quaternion.identity).GetComponent<CellView>();

            _flaggedCells.Add(coords, new FlagCellInfo() 
            {
                IsFlagged = true,
                View = cellInstance
            });
        }

        bool isFlagged = _flaggedCells[coords].IsFlagged;

        _flaggedCells[coords].View.Initialize(isFlagged ? _config.Sprites[-2] : null);

        UpdateGridInfo(coords);
    }

    private void UpdateGridInfo(Vector3Int coords)
    {
        Vector3Int origin = GridHelper.ConvertToGridCoords(coords, _config.Size);

        if (!_spawnedGrids.ContainsKey(origin))
        {
            var gridView = _gridFactory.Create();
            gridView.transform.position = origin;
            gridView.Initialize(_config.Size * _config.Size);

            _spawnedGrids.Add(origin, gridView);
        }

        if (_openedCells.Contains(coords))
        {
            if (_mineList.Contains(coords))
                _spawnedGrids[origin].SetState(GridStates.Death);

            _spawnedGrids[origin].AddValue(1);
        }
        else if (_flaggedCells.ContainsKey(coords))
        {
            if (_flaggedCells[coords].IsFlagged)
                _spawnedGrids[origin].AddValue(1);
            else
                _spawnedGrids[origin].AddValue(-1);
        }
    }

    private async UniTask GenerateGridMines(Vector3Int origin, CancellationToken token)
    {
        _gridCoords.Add(origin);

        int minesCounter = 0;

        while (minesCounter < _config.MinesPerChunk)
        {
            Vector3Int mineCoords = new Vector3Int(origin.x + Random.Range(0, _config.Size), origin.y + Random.Range(0, _config.Size));

            if (_mineList.Contains(mineCoords)) continue;
            if (_config.StartCells.ContainsKey(mineCoords)) continue;

            bool canPlaceMine = true;

            foreach(var neighbour in _neighbours)
            {
                if (_openedCells.Contains(mineCoords + neighbour))
                {
                    canPlaceMine = false;
                    break;
                }
            }

            if (canPlaceMine)
            {
                _mineList.Add(mineCoords);
                minesCounter++;
            }
        }

        await UniTask.Yield(token);

        Debug.Log($"[GenerateGridMines] Origin:{origin} Mines:{_mineList.Count}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (var item in _mineList)
        {
            Gizmos.DrawCube(item, Vector3.one);
        }
    }
}