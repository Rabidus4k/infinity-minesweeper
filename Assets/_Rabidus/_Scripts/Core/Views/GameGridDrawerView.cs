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
    private Dictionary<Vector3Int, CellInfo> _cells = new Dictionary<Vector3Int, CellInfo>();

    protected IInputViewModel _inputViewModel;
    protected IGameViewModel _gameViewModel;
    private IScoreViewModel _scoreViewModel;
    private UINotificationManager _notificationManager;

    [Inject]
    private void Construct(IInputViewModel inputViewModel, IGameViewModel gameViewModel, IScoreViewModel scoreViewModel, UINotificationManager uINotificationManager)
    {
        _notificationManager = uINotificationManager;

        _inputViewModel = inputViewModel;
        _inputViewModel.LMBCoords.OnChanged += HandleMouseClick;
        _inputViewModel.RMBCoords.OnChanged += HandleRightClick;
        _gameViewModel = gameViewModel;
        _scoreViewModel = scoreViewModel;
        foreach (var item in _gameViewModel.Config.Value.StartCells)
        {
            _cells.Add(item.Key, new CellInfo(0));
        }

        if (_gameViewModel.Config.Value.Seed != 0)
            Random.InitState(_gameViewModel.Config.Value.Seed);
    }

    private void Start()
    {
        DrawChunk(Vector3Int.zero, _cells).Forget();
        HandleClick(Vector3Int.zero);
    }

    protected void OnDisable()
    {
        _inputViewModel.LMBCoords.OnChanged -= HandleMouseClick;
        _inputViewModel.RMBCoords.OnChanged -= HandleRightClick;
    }

    private void HandleRightClick(Vector3Int coords)
    {
        if (_cells.ContainsKey(coords))
        {
            if (_cells[coords].IsOpened) return;

            FlagTile(coords);
        }
        else
        {
            Vector3Int origin = GridHelper.ConvertToGridCoords(coords, _gameViewModel.Config.Value.Size);

            var chunk = ChunkCreatorHelper.GenerateChunk(_cells, origin, _gameViewModel.Config.Value);

            foreach (var cell in chunk)
            {
                if (_cells.ContainsKey(cell.Key)) continue;
                _cells.Add(cell.Key, cell.Value);
            }

            DrawChunk(origin, chunk).Forget();

            HandleRightClick(coords);
        }
    }

    private void HandleMouseClick(Vector3Int coords)
    {
        if (CheckNeighbours(coords))
            HandleClick(coords);
        else
            _notificationManager.SendNotification(_notificationInfo);
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

        if (_cells.ContainsKey(coords))
        {
            if (_cells[coords].IsFlagged) return;

            if (_cells[coords].IsOpened && _cells[coords].Value != 0 && CheckFlagsAround(coords))
            {
                await UniTask.WaitForSeconds(0.01f);
                
                if (recurce)
                    OpenTileAround(coords, clickBuffer, false);
            }
            else if (_cells[coords].Value == 0 && _cells[coords].IsOpened == false)
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
            var chunk = ChunkCreatorHelper.GenerateChunk(_cells, origin, _gameViewModel.Config.Value);

            foreach (var cell in chunk)
            {
                if (_cells.ContainsKey(cell.Key)) continue;
                _cells.Add(cell.Key, cell.Value);
            }

            DrawChunk(origin, chunk).Forget();

            HandleClick(coords);
        }
    }

    private void FlagTile(Vector3Int coords)
    {
        bool wasFlagged = _cells[coords].IsFlagged;
        _cells[coords].IsFlagged = !wasFlagged;

        if (_cells[coords].IsFlagged)
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

        _gameViewModel.SelectCell(new Cell(coords, _cells[coords]));
    }

    private void OpenTile(Vector3Int coords)
    {
        if (_cells[coords].IsOpened) return;
        _cells[coords].IsOpened = true;

        if (_spawnedCell.ContainsKey(coords))
        {
            var cellInstance = _spawnedCell[coords];
            cellInstance.Initialize(_sprites[_cells[coords].Value]);
        }

        _scoreViewModel.AddScore(1);
        _gameViewModel.SelectCell(new Cell(coords, _cells[coords]));
    }

    private bool CheckFlagsAround(Vector3Int coords)
    {
        int counter = 0;
        int needCount = _cells[coords].Value;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                var checkCoords = new Vector3Int(coords.x + x, coords.y + y);

                if (_cells.ContainsKey(checkCoords))
                {
                    if (_cells[checkCoords].IsFlagged)
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

                if (_cells.ContainsKey(checkCoords) && _cells[checkCoords].IsOpened)
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

    private void OnDrawGizmos()
    {

        foreach (var item in _cells)
        {
            if (item.Value.Value == -1)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(item.Key, 0.5f);
        }
    }
}
