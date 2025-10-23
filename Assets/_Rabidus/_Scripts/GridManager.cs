using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

public class GridManager : MonoBehaviour
{
    public GridView GridView;
    public SerializedDictionary<Vector3Int, CellInfo> Cells = new SerializedDictionary<Vector3Int, CellInfo>();
    public int ChunkSize = 8;
    public int MinesPerChunk = 15;

    protected IInputViewModel _viewModel;

    private void Awake()
    {
        Random.InitState(123);
        GridView.DrawChunk(Vector3Int.zero, Cells);
        HandleClick(Vector3Int.zero);
    }

    [Inject]
    private void Construct(IInputViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.LMBCoords.OnChanged += HandleClick;
    }

    protected void OnDisable()
    {
        _viewModel.LMBCoords.OnChanged -= HandleClick;
    }

    public void HandleClick(Vector3Int coords)
    {
        if (Cells.ContainsKey(coords))
        {
            if (Cells[coords].Value == 0)
            {
                if (Cells[coords].IsOpened) return;

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
            Vector3Int origin = new Vector3Int(Mathf.FloorToInt((float)coords.x / ChunkSize) * ChunkSize, Mathf.FloorToInt((float)coords.y / ChunkSize) * ChunkSize, 0);

            GenerateChunk(origin);
            HandleClick(coords);
        }
    }

    private void OpenTile(Vector3Int coords)
    {
        Cells[coords].IsOpened = true;
        GridView.RefreshTile(coords, Cells[coords].Value);
    }

    public void GenerateChunk(Vector3Int origin)
    {
        Dictionary<Vector3Int, CellInfo> chunk = new Dictionary<Vector3Int, CellInfo>();

        //Empty map
        for (int x = -1; x < ChunkSize + 1; x++)
        {
            for (int y = -1; y < ChunkSize + 1; y++)
            {
                Vector3Int coords = new Vector3Int(origin.x + x, origin.y + y);
                
                if (Cells.ContainsKey(coords))
                    chunk.Add(coords, Cells[coords]);
                else
                {
                    CellInfo cell = new CellInfo(0);
                    chunk.Add(coords, cell);
                }
            }
        }

        //Place mines
        for (int i = 0; i < MinesPerChunk; i++)
        {
            Vector3Int mineCoords = new Vector3Int(origin.x + Random.Range(0, ChunkSize), origin.y + Random.Range(0, ChunkSize));

            if (Cells.ContainsKey(mineCoords)) continue;

            if (Cells.ContainsKey(mineCoords + new Vector3Int(-1, 1))) continue;
            if (Cells.ContainsKey(mineCoords + new Vector3Int(-1, 0))) continue;
            if (Cells.ContainsKey(mineCoords + new Vector3Int(-1, -1))) continue;

            if (Cells.ContainsKey(mineCoords + new Vector3Int(0, 1))) continue;
            if (Cells.ContainsKey(mineCoords + new Vector3Int(0, -1))) continue;

            if (Cells.ContainsKey(mineCoords + new Vector3Int(1, 1))) continue;
            if (Cells.ContainsKey(mineCoords + new Vector3Int(1, 0))) continue;
            if (Cells.ContainsKey(mineCoords + new Vector3Int(1, -1))) continue;

            chunk[mineCoords].Value = -1;
        }

        //Fill values
        for (int x = 0; x < ChunkSize; x++)
        {
            for (int y = 0; y < ChunkSize; y++)
            {
                Vector3Int coords = new Vector3Int(origin.x + x, origin.y + y);
                var cell = chunk[coords];

                if (cell.Value != -1) continue;

                for (int dx = -1; dx <= 1; dx++)
                {
                    int nx = x + dx;

                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int ny = y + dy;

                        Vector3Int neighbourCoords = new Vector3Int(origin.x + nx, origin.y + ny);
                        var neighbourCell = chunk[neighbourCoords];

                        if (neighbourCell.Value == -1) continue;

                        neighbourCell.Value++;
                    }
                }
            }
        }

        foreach (var cell in chunk)
        {
            if (Cells.ContainsKey(cell.Key)) continue;
            Cells.Add(cell.Key, cell.Value);
        }

        GridView.DrawChunk(origin, chunk);
    }

    private void OnDrawGizmos()
    {
        
        foreach (var item in Cells)
        {
            if (item.Value.Value == -1)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.yellow;

            Gizmos.DrawSphere(item.Key, 0.5f);
        }
    }
}
