using System.Collections.Generic;
using UnityEngine;

public static class ChunkCreatorHelper
{
    public static Dictionary<Vector3Int, CellInfo> GenerateChunk(Dictionary<Vector3Int, CellInfo> cells, Vector3Int origin, IGameConfig config)
    {
        Dictionary<Vector3Int, CellInfo> chunk = new Dictionary<Vector3Int, CellInfo>();

        for (int x = -1; x < config.Size + 1; x++)
        {
            for (int y = -1; y < config.Size + 1; y++)
            {
                Vector3Int coords = new Vector3Int(origin.x + x, origin.y + y);

                if (cells.ContainsKey(coords))
                    chunk.Add(coords, cells[coords]);
                else
                {
                    CellInfo cell = new CellInfo()
                    {
                        IsFlagged = false,
                        IsOpened = false,
                        Value = 0
                    };
                    chunk.Add(coords, cell);
                }
            }
        }

        //Place mines
        for (int i = 0; i < config.MinesPerChunk; i++)
        {
            Vector3Int mineCoords = new Vector3Int(origin.x + Random.Range(0, config.Size), origin.y + Random.Range(0, config.Size));

            if (cells.ContainsKey(mineCoords)) continue;

            if (cells.ContainsKey(mineCoords + new Vector3Int(-1, 1))) continue;
            if (cells.ContainsKey(mineCoords + new Vector3Int(-1, 0))) continue;
            if (cells.ContainsKey(mineCoords + new Vector3Int(-1, -1))) continue;

            if (cells.ContainsKey(mineCoords + new Vector3Int(0, 1))) continue;
            if (cells.ContainsKey(mineCoords + new Vector3Int(0, -1))) continue;

            if (cells.ContainsKey(mineCoords + new Vector3Int(1, 1))) continue;
            if (cells.ContainsKey(mineCoords + new Vector3Int(1, 0))) continue;
            if (cells.ContainsKey(mineCoords + new Vector3Int(1, -1))) continue;

            chunk[mineCoords] = new CellInfo()
            {
                IsFlagged = false,
                IsOpened = false,
                Value = -1
            };
        }

        //Fill values
        for (int x = 0; x < config.Size; x++)
        {
            for (int y = 0; y < config.Size; y++)
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

                        if (chunk[neighbourCoords].Value == -1) continue;

                        chunk[neighbourCoords] = new CellInfo()
                        {
                            IsFlagged = false,
                            IsOpened = false,
                            Value = chunk[neighbourCoords].Value + 1
                        };
                    }
                }
            }
        }

        return chunk;
    }
}
