using UnityEngine;

public static class GridHelper
{
    public static Vector3Int ConvertToGridCoords(Vector3Int coords, int size)
    {
        return new Vector3Int
        (
            Mathf.FloorToInt((float)coords.x / size) * size,
            Mathf.FloorToInt((float)coords.y / size) * size,
            0
        );
    }
}
