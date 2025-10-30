using Cysharp.Threading.Tasks;
using Lean.Pool;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VInspector;
using Zenject;

public class GameGridDrawerView : MonoBehaviour
{
    [SerializeField] private UIPanel _loadingPanel;

    [SerializeField] private LeanGameObjectPool _cellPool;
    [SerializeField] private SerializedDictionary<int, Sprite> _sprites = new SerializedDictionary<int, Sprite>();
    [SerializeField] private UINotificationInfo _notificationInfo;
    [SerializeField] private int _drawCellsPerFrame = 8;

    private Dictionary<Vector3Int, CellView> _spawnedCell = new Dictionary<Vector3Int, CellView>();
    private Dictionary<Vector3Int, GridView> _gridViews = new Dictionary<Vector3Int, GridView>();

    private GridView.Factory _gridFactory;

    private IInputViewModel _inputViewModel;
    private IGameViewModel _gameViewModel;
    private IScoreViewModel _scoreViewModel;

    private UINotificationManager _notificationManager;
    private SoundManager _soundManager;
    private ISaveService _saveService;

    private bool _canPlaceTile = true;
    

    [Inject]
    private void Construct
        (
            IInputViewModel inputViewModel, 
            IGameViewModel gameViewModel, 
            IScoreViewModel scoreViewModel,
            UINotificationManager uINotificationManager,
            GridView.Factory factory,
            ISaveService saveService,
            SoundManager soundManager
        )
    {
        _saveService = saveService;
        _soundManager = soundManager;
        _notificationManager = uINotificationManager;

        _inputViewModel = inputViewModel;
        _inputViewModel.LMBCoords.OnChanged += HandleLeftClick;
        _inputViewModel.RMBCoords.OnChanged += HandleRightClick;
        _gameViewModel = gameViewModel;
        _scoreViewModel = scoreViewModel;

        _gridFactory = factory;

        if (_gameViewModel.Config.Value.Seed != 0)
            Random.InitState(_gameViewModel.Config.Value.Seed);

        Debug.Log($"Seed: {Random.state}");

        _saveService.IsLoaded.OnChanged += (bool value) => PrepareGameField(_cts.Token).Forget();
    }

    private CancellationTokenSource _cts;

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        _inputViewModel.LMBCoords.OnChanged -= HandleLeftClick;
        _inputViewModel.RMBCoords.OnChanged -= HandleRightClick;

        _saveService.IsLoaded.OnChanged -= (bool value) => PrepareGameField(_cts.Token).Forget();

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

    private async UniTask PrepareGameField(CancellationToken token)
    {
        _loadingPanel.ShowPanel();

        _canPlaceTile = false;
        await DrawChunkAsync(Vector3Int.zero, _gameViewModel.Cells.Value, token);
        await HandleClickAsync(_gameViewModel.Config.Value.OriginCell, token);

        foreach (var item in _gameViewModel.Cells.Value)
        {
            UpdateGridInfo(item.Key);
        }

        _canPlaceTile = true;

        await UniTask.WaitForSeconds(0.5f);

        _loadingPanel.HidePanel();
    }

    private void HandleRightClick(Vector3Int coords)
    {
        if (!_canPlaceTile) return;

        HandleRightClickAsync(coords, _cts.Token).Forget();
    }

    private async UniTask HandleRightClickAsync(Vector3Int coords, CancellationToken token)
    {
        if (_gameViewModel.Cells.Value.ContainsKey(coords))
        {
            if (_gameViewModel.Cells.Value[coords].IsOpened) return;

            FlagTile(coords);
            UpdateGridInfo(coords);
        }
        else
        {
            Vector3Int origin = GridHelper.ConvertToGridCoords(coords, _gameViewModel.Config.Value.Size);
            var chunk = ChunkCreatorHelper.GenerateChunk(_gameViewModel.Cells.Value, origin, _gameViewModel.Config.Value);

            foreach (var cell in chunk)
            {
                if (_gameViewModel.Cells.Value.ContainsKey(cell.Key)) continue;
                _gameViewModel.Cells.Value.Add(cell.Key, cell.Value);
            }

            await DrawChunkAsync(origin, chunk, token);

            HandleRightClick(coords);
        }
    }

    private void HandleLeftClick(Vector3Int coords)
    {
        if (!_canPlaceTile) return;

        if (CheckNeighbours(coords))
        {
            HandleClickAsync(coords, _cts.Token).Forget();
        }
        else
            _notificationManager.SendNotification(_notificationInfo);
    }

    private void UpdateGridInfo(Vector3Int coords)
    {
        Vector3Int origin = GridHelper.ConvertToGridCoords(coords, _gameViewModel.Config.Value.Size);

        if (!_gridViews.ContainsKey(origin))
        {
            var gridView = _gridFactory.Create();
            gridView.transform.position = origin;

            gridView.Initialize(_gameViewModel.Config.Value.Size * _gameViewModel.Config.Value.Size);

            _gridViews.Add(origin, gridView);
        }

        _gridViews[origin].RefreshCell(coords);
    }

    private async UniTask HandleClickAsync(Vector3Int coords, CancellationToken token)
    {
        await HandleClick(coords, new List<Vector3Int>(), token);
    }

    private async UniTask HandleClick(Vector3Int coords, List<Vector3Int> clickBuffer, CancellationToken token, bool recurce = true)
    {
        if (clickBuffer != null)
        {
            if (clickBuffer.Contains(coords)) return;
            clickBuffer.Add(coords);
        }

        if (_gameViewModel.Cells.Value.ContainsKey(coords))
        {
            if (_gameViewModel.Cells.Value[coords].IsFlagged) return;

            if (_gameViewModel.Cells.Value[coords].IsOpened && _gameViewModel.Cells.Value[coords].Value != 0 && CheckFlagsAround(coords))
            {
                await UniTask.WaitForSeconds(0.01f, cancellationToken: token);
                
                if (recurce)
                    await OpenTileAround(coords, clickBuffer, token, false);
            }
            else if (_gameViewModel.Cells.Value[coords].Value == 0 && _gameViewModel.Cells.Value[coords].IsOpened == false)
            {
                OpenTile(coords);

                await OpenTileAround(coords, clickBuffer, token);
            }
            else
            {
                OpenTile(coords);
            }
        }
        else
        {
            Vector3Int origin = GridHelper.ConvertToGridCoords(coords, _gameViewModel.Config.Value.Size);
            var chunk = ChunkCreatorHelper.GenerateChunk(_gameViewModel.Cells.Value, origin, _gameViewModel.Config.Value);

            foreach (var cell in chunk)
            {
                if (_gameViewModel.Cells.Value.ContainsKey(cell.Key)) continue;
                _gameViewModel.Cells.Value.Add(cell.Key, cell.Value);
            }

            await DrawChunkAsync(origin, chunk, token);
            await HandleClick(coords, new List<Vector3Int>(), token);
        }

        UpdateGridInfo(coords);
    }

    private void FlagTile(Vector3Int coords)
    {
        bool wasFlagged = _gameViewModel.Cells.Value[coords].IsFlagged;

        var cellInfo = new CellInfo()
        {
            Value = _gameViewModel.Cells.Value[coords].Value,
            IsOpened = _gameViewModel.Cells.Value[coords].IsOpened,
            IsFlagged = !wasFlagged,
        };

        _gameViewModel.Cells.Value[coords] = cellInfo;

        if (_gameViewModel.Cells.Value[coords].IsFlagged)
        {
            if (_spawnedCell.ContainsKey(coords))
            {
                var cellInstance = _spawnedCell[coords];
                cellInstance.Initialize(_sprites[-2]);
            }
        }
        else
        {
            if (_spawnedCell.ContainsKey(coords))
            {
                var cellInstance = _spawnedCell[coords];
                cellInstance.Initialize(null);
            }
        }

        _soundManager.PlaySound("OpenCell");
    }

    private void OpenTile(Vector3Int coords)
    {
        if (_gameViewModel.Cells.Value[coords].IsOpened) return;
        
        var cellInfo = new CellInfo()
        {
            Value = _gameViewModel.Cells.Value[coords].Value,
            IsOpened = true,
            IsFlagged = _gameViewModel.Cells.Value[coords].IsFlagged,
        };

        _gameViewModel.Cells.Value[coords] = cellInfo;

        if (_spawnedCell.ContainsKey(coords))
        {
            var cellInstance = _spawnedCell[coords];
            cellInstance.Initialize(_sprites[_gameViewModel.Cells.Value[coords].Value]);
        }

        _soundManager.PlaySound("OpenCell");
        _scoreViewModel.AddScore(1);
    }

    private bool CheckFlagsAround(Vector3Int coords)
    {
        int counter = 0;
        int needCount = _gameViewModel.Cells.Value[coords].Value;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                var checkCoords = new Vector3Int(coords.x + x, coords.y + y);

                if (_gameViewModel.Cells.Value.ContainsKey(checkCoords))
                {
                    if (_gameViewModel.Cells.Value[checkCoords].IsFlagged || (_gameViewModel.Cells.Value[checkCoords].IsOpened && _gameViewModel.Cells.Value[checkCoords].Value == -1))
                        counter++;
                }
            }
        }

        return counter == needCount;
    }

    private bool CheckNeighbours(Vector3Int coords)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                var checkCoords = new Vector3Int(coords.x + x, coords.y + y);

                if (_gameViewModel.Cells.Value.ContainsKey(checkCoords) && _gameViewModel.Cells.Value[checkCoords].IsOpened)
                    return true;
            }
        }

        return false;
    }

    private async UniTask OpenTileAround(Vector3Int coords, List<Vector3Int> clickBuffer, CancellationToken token, bool recurce = true)
    {
        await HandleClick(new Vector3Int(coords.x - 1, coords.y), clickBuffer, token, recurce);
        await HandleClick(new Vector3Int(coords.x + 1, coords.y), clickBuffer, token, recurce);
        await HandleClick(new Vector3Int(coords.x, coords.y - 1), clickBuffer, token, recurce);
        await HandleClick(new Vector3Int(coords.x, coords.y + 1), clickBuffer, token, recurce);

        await HandleClick(new Vector3Int(coords.x - 1, coords.y + 1), clickBuffer, token, recurce);
        await HandleClick(new Vector3Int(coords.x - 1, coords.y - 1), clickBuffer, token, recurce);
        await HandleClick(new Vector3Int(coords.x + 1, coords.y + 1), clickBuffer, token, recurce);
        await HandleClick(new Vector3Int(coords.x + 1, coords.y - 1), clickBuffer, token, recurce);
    }

    public async UniTask DrawChunkAsync(Vector3Int origin, Dictionary<Vector3Int, CellInfo> chunk, CancellationToken token)
    {
        int counter = 0;
        GameObject chunkOn = new GameObject("Chunk");
        var parent = chunkOn.transform;

        foreach (var item in chunk)
        {
            if (_spawnedCell.ContainsKey(item.Key)) continue;

            var cellInstance = _cellPool.Spawn(item.Key, Quaternion.identity, parent).GetComponent<CellView>();
            _spawnedCell.Add(item.Key, cellInstance);

            if (item.Value.IsOpened)
            {
                cellInstance.Initialize(_sprites[item.Value.Value]);
                _scoreViewModel.AddScore(1);
            }
            else
            {
                if (item.Value.IsFlagged)
                {
                    cellInstance.Initialize(_sprites[-2]);
                }
                else
                {
                    cellInstance.Initialize(null);
                }
            }

            counter++;

            if (counter >= _drawCellsPerFrame)
            {
                counter = 0;
                await UniTask.WaitForEndOfFrame(token);
            }
        }

    }
}
