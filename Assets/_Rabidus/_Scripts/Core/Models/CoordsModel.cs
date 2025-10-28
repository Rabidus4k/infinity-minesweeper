using System.Collections.Generic;

public class CoordsModel : ICoordsModel
{
    public List<CoordsInfo> CoordsInfo { get; private set; }

    public CoordsModel()
    {
        CoordsInfo = new List<CoordsInfo>();
    }

    public void LoadData(object data)
    {
        var coordsInfo = (CoordsSaveData)data;

        if (coordsInfo.CoordsInfo == null) return;

        foreach (var item in coordsInfo.CoordsInfo)
        {
            CoordsInfo.Add(item);
        }
    }
}

public interface ICoordsModel : ILoadable
{
    public List<CoordsInfo> CoordsInfo { get; }
}
