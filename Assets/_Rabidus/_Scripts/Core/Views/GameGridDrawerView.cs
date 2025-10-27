using Cysharp.Threading.Tasks;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

public class GameGridDrawerView : MonoBehaviour
{
    [SerializeField] private LeanGameObjectPool _cellPool;
    [SerializeField] private SerializedDictionary<int, Sprite> _sprites = new SerializedDictionary<int, Sprite>();
    [SerializeField] private UINotificationInfo _notificationInfo;

    private Dictionary<Vector3Int, CellView> _spawnedCell = new Dictionary<Vector3Int, CellView>();
    private Dictionary<Vector3Int, GridView> _gridViews = new Dictionary<Vector3Int, GridView>();

    private GridView.Factory _gridFactory;

    private IInputViewModel _inputViewModel;
    private IGameViewModel _gameViewModel;
    private IScoreViewModel _scoreViewModel;
    private UINotificationManager _notificationManager;

    [Inject]
    private void Construct(IInputViewModel inputViewModel, IGameViewModel gameViewModel, IScoreViewModel scoreViewModel, UINotificationManager uINotificationManager, GridView.Factory factory)
    {
        _notificationManager = uINotificationManager;

        _inputViewModel = inputViewModel;
        _inputViewModel.LMBCoords.OnChanged += HandleMouseClick;
        _inputViewModel.RMBCoords.OnChanged += HandleRightClick;
        _gameViewModel = gameViewModel;
        _scoreViewModel = scoreViewModel;

        _gridFactory = factory;

        if (_gameViewModel.Config.Value.Seed != 0)
            Random.InitState(_gameViewModel.Config.Value.Seed);
    }

    private void Start()
    {
        DrawChunk(Vector3Int.zero, _gameViewModel.Cells.Value).Forget();
        HandleClick(Vector3Int.zero);
    }

    protected void OnDisable()
    {
        _inputViewModel.LMBCoords.OnChanged -= HandleMouseClick;
        _inputViewModel.RMBCoords.OnChanged -= HandleRightClick;
    }

    private void HandleRightClick(Vector3Int coords)
    {
        if (_gameViewModel.Cells.Value.ContainsKey(coords))
        {
            if (_gameViewModel.Cells.Value[coords].IsOpened) return;

            FlagTile(coords);
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

            DrawChunk(origin, chunk).Forget();

            HandleRightClick(coords);
        }

        UpdateGridInfo(coords);
    }

    private void HandleMouseClick(Vector3Int coords)
    {
        if (CheckNeighbours(coords))
        {
            HandleClick(coords);
        }
        else
            _notificationManager.SendNotification(_notificationInfo);
    }

    private void UpdateGridInfo(Vector3Int coords)
    {
        Vector3Int origin = GridHelper.ConvertToGridCoords(coords, _gameViewModel.Config.Value.Size);
        Debug.Log($"Update grid: {origin}");
        if (!_gridViews.ContainsKey(origin))
        {
            var gridView = _gridFactory.Create();
            gridView.transform.position = origin;

            gridView.Initialize(_gameViewModel.Config.Value.Size * _gameViewModel.Config.Value.Size);

            _gridViews.Add(origin, gridView);
        }

        _gridViews[origin].RefreshCell(coords);
    }

    private void HandleClick(Vector3Int coords)
    {
        HandleClick(coords, new List<Vector3Int>()).Forget();
    }

    private async UniTask HandleClick(Vector3Int coords, List<Vector3Int> clickBuffer, bool recurce = true)
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
                await UniTask.WaitForSeconds(0.01f);
                
                if (recurce)
                    OpenTileAround(coords, clickBuffer, false);
            }
            else if (_gameViewModel.Cells.Value[coords].Value == 0 && _gameViewModel.Cells.Value[coords].IsOpened == false)
            {
                OpenTile(coords);
                await UniTask.WaitForSeconds(0.01f);

                OpenTileAround(coords, clickBuffer);
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

            DrawChunk(origin, chunk).Forget();

            HandleClick(coords);
        }

        UpdateGridInfo(coords);
    }

    private void FlagTile(Vector3Int coords)
    {
        bool wasFlagged = _gameViewModel.Cells.Value[coords].IsFlagged;

        _gameViewModel.Cells.Value[coords] = new CellInfo()
        {
            Value = _gameViewModel.Cells.Value[coords].Value,
            IsOpened = _gameViewModel.Cells.Value[coords].IsOpened,
            IsFlagged = !wasFlagged,
        };


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
    }

    private void OpenTile(Vector3Int coords)
    {
        if (_gameViewModel.Cells.Value[coords].IsOpened) return;
        _gameViewModel.Cells.Value[coords] = new CellInfo()
        {
            Value = _gameViewModel.Cells.Value[coords].Value,
            IsOpened = true,
            IsFlagged = _gameViewModel.Cells.Value[coords].IsFlagged,
        };

        if (_spawnedCell.ContainsKey(coords))
        {
            var cellInstance = _spawnedCell[coords];
            cellInstance.Initialize(_sprites[_gameViewModel.Cells.Value[coords].Value]);
        }

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
                    if (_gameViewModel.Cells.Value[checkCoords].IsFlagged)
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

    private void OpenTileAround(Vector3Int coords, List<Vector3Int> clickBuffer, bool recurce = true)
    {
        HandleClick(new Vector3Int(coords.x - 1, coords.y), clickBuffer, recurce).Forget();
        HandleClick(new Vector3Int(coords.x + 1, coords.y), clickBuffer, recurce).Forget();
        HandleClick(new Vector3Int(coords.x, coords.y - 1), clickBuffer, recurce).Forget();
        HandleClick(new Vector3Int(coords.x, coords.y + 1), clickBuffer, recurce).Forget();

        HandleClick(new Vector3Int(coords.x - 1, coords.y + 1), clickBuffer, recurce).Forget();
        HandleClick(new Vector3Int(coords.x - 1, coords.y - 1), clickBuffer, recurce).Forget();
        HandleClick(new Vector3Int(coords.x + 1, coords.y + 1), clickBuffer, recurce).Forget();
        HandleClick(new Vector3Int(coords.x + 1, coords.y - 1), clickBuffer, recurce).Forget();
    }

    public async UniTask DrawChunk(Vector3Int origin, Dictionary<Vector3Int, CellInfo> chunk)
    {
        GameObject chunkOn = new GameObject("Chunk");
        var parent = chunkOn.transform;

        foreach (var item in chunk)
        {
            if (_spawnedCell.ContainsKey(item.Key)) continue;

            var cellInstance = _cellPool.Spawn(item.Key, Quaternion.identity, parent).GetComponent<CellView>();
            _spawnedCell.Add(item.Key, cellInstance);

            cellInstance.Initialize(null);
            //cellInstance.Initialize(_sprites[item.Value.Value]);
        }

        await UniTask.WaitForEndOfFrame();
    }
}
