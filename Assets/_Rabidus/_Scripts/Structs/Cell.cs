using UnityEngine;

public struct Cell
{
    public Vector3Int Coords;
    public CellInfo Info;

    public Cell(Vector3Int coords, CellInfo info)
    {
        Coords = coords;
        Info = info;
    }
}
