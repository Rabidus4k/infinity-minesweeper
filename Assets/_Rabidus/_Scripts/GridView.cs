using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class GridView : MonoBehaviour
{
    private Dictionary<Vector3Int, CellView> _spawnedCell = new Dictionary<Vector3Int, CellView>();

    [SerializeField] private CellView _cell;

    [SerializeField] private SerializedDictionary<int, Sprite> _sprites = new SerializedDictionary<int, Sprite>();

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
}
