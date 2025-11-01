using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Payload
{
    public static string ToString(List<Vector3Int> cells)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var cell in cells)
        {
            stringBuilder.Append($"{cell.x}.{cell.y} ");
        }

        return stringBuilder.ToString();
    }

    public static List<Vector3Int> FromString(string data)
    {
        return 
            Regex.Matches(data, @"-?\d+\.-?\d+").Cast<Match>()
            .Select(m => m.Value.Split('.'))
            .Select(p => new Vector3Int(
                int.Parse(p[0], CultureInfo.InvariantCulture),
                int.Parse(p[1], CultureInfo.InvariantCulture),
                0))
            .ToList();
    }
}
