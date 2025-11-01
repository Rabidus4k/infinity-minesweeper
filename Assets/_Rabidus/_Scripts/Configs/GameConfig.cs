using UnityEngine;
using VInspector;

[CreateAssetMenu(menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject, IGameConfig
{
    [field: SerializeField] public SerializedDictionary<int, Sprite> Sprites { get; private set; } = new SerializedDictionary<int, Sprite>();
    [field: SerializeField] public Vector3Int OriginCell { get; private set; }
    [field: SerializeField] public SerializedDictionary<Vector3Int, CellInfo> StartCells { get; private set; } = new SerializedDictionary<Vector3Int, CellInfo>();
    [field: SerializeField] public int Size { get; private set; } = 8;
    [field: SerializeField] public int Seed { get; private set; } = 123;
    [field: SerializeField] public int MinesPerChunk { get; private set; } = 20;
}

public interface IGameConfig
{
    public SerializedDictionary<int, Sprite> Sprites { get; }
    public Vector3Int OriginCell { get; }
    public SerializedDictionary<Vector3Int, CellInfo> StartCells { get; }
    public int Size { get; }
    public int Seed { get; }
    public int MinesPerChunk { get; }
}
