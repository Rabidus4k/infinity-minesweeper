using UnityEngine;
using VInspector;

[CreateAssetMenu(menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject, IGameConfig
{
    [field: SerializeField] public SerializedDictionary<Vector3Int, CellInfo> StartCells { get; private set; } = new SerializedDictionary<Vector3Int, CellInfo>();
    [field: SerializeField] public int Size { get; private set; } = 8;
    [field: SerializeField] public int MinesPerChunk { get; private set; } = 20;
}

public interface IGameConfig
{
    public SerializedDictionary<Vector3Int, CellInfo> StartCells { get; }
    public int Size { get; }
    public int MinesPerChunk { get; }
}
