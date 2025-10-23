using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

public class GameGridDrawerView : MonoBehaviour
{
    [SerializeField] private CellView _cell;
    [SerializeField] private SerializedDictionary<int, Sprite> _sprites = new SerializedDictionary<int, Sprite>();

    private Dictionary<Vector3Int, CellView> _spawnedCell = new Dictionary<Vector3Int, CellView>();
    private Dictionary<Vector3Int, CellInfo> _cells = new Dictionary<Vector3Int, CellInfo>();

    protected IInputViewModel _inputViewModel;
    protected IGameViewModel _gameViewModel;

    [Inject]
    private void Construct(IInputViewModel inputViewModel, IGameViewModel gameViewModel)
    {
        _inputViewModel = inputViewModel;
        _inputViewModel.LMBCoords.OnChanged += HandleClick;

        _gameViewModel = gameViewModel;

        foreach (var item in _gameViewModel.Config.Value.StartCells)
        {
            _cells.Add(item.Key, item.Value);
        }
    }

    private void Awake()
    {
        Random.InitState(123);

        DrawChunk(Vector3Int.zero, _cells);
        HandleClick(Vector3Int.zero);
    }

    protected void OnDisable()
    {
        _inputViewModel.LMBCoords.OnChanged -= HandleClick;
    }

    public void HandleClick(Vector3Int coords)
    {
        if (_cells.ContainsKey(coords))
        {
            if (_cells[coords].Value == 0)
            {
                if (_cells[coords].IsOpened) return;

                OpenTile(coords);

                HandleClick(new Vector3Int(coords.x - 1, coords.y));
                HandleClick(new Vector3Int(coords.x + 1, coords.y));
                HandleClick(new Vector3Int(coords.x, coords.y - 1));
                HandleClick(new Vector3Int(coords.x, coords.y + 1));

                HandleClick(new Vector3Int(coords.x - 1, coords.y + 1));
                HandleClick(new Vector3Int(coords.x - 1, coords.y - 1));
                HandleClick(new Vector3Int(coords.x + 1, coords.y + 1));
                HandleClick(new Vector3Int(coords.x + 1, coords.y - 1));
            }
            else
            {
                OpenTile(coords);
            }
        }
        else
        {
            Vector3Int origin = new Vector3Int
            (
                Mathf.FloorToInt((float)coords.x / _gameViewModel.Config.Value.Size) * _gameViewModel.Config.Value.Size,
                Mathf.FloorToInt((float)coords.y / _gameViewModel.Config.Value.Size) * _gameViewModel.Config.Value.Size,
                0
            );

            var chunk = ChunkCreatorHelper.GenerateChunk(_cells, origin, _gameViewModel.Config.Value);
            
            foreach (var cell in chunk)
            {
                if (_cells.ContainsKey(cell.Key)) continue;
                _cells.Add(cell.Key, cell.Value);
            }

            DrawChunk(origin, chunk);

            HandleClick(coords);
        }
    }

    private void OpenTile(Vector3Int coords)
    {
        _cells[coords].IsOpened = true;
        RefreshTile(coords, _cells[coords].Value);
    }



    public void RefreshTile(Vector3Int coords, int value)
    {
        if (_spawnedCell.ContainsKey(coords))
        {
            var cellInstance = _spawnedCell[coords];
            cellInstance.Initialize(_sprites[value]);
        }
    }

    public void DrawChunk(Vector3Int origin, Dictionary<Vector3Int, CellInfo> chunk)
    {
        GameObject chunkOn = new GameObject("Chunk");
        var parent = chunkOn.transform;

        foreach (var item in chunk)
        {
            if (_spawnedCell.ContainsKey(item.Key)) continue;

            var cellInstance = Instantiate(_cell, item.Key, Quaternion.identity, parent);
            _spawnedCell.Add(item.Key, cellInstance);

            cellInstance.Initialize(null);
            //cellInstance.Initialize(_sprites[item.Value.Value]);
        }
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
