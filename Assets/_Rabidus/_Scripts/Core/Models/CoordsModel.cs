using System.Collections.Generic;

public class CoordsModel : ICoordsModel
{
    public bool IsLoaded { get; private set; }
    public List<CoordsInfo> CoordsInfo { get; private set; }

    public CoordsModel()
    {
        CoordsInfo = new List<CoordsInfo>();
    }

    public void LoadData(object data)
    {
        if (data != null)
        {
            var coordsInfo = (CoordsSaveData)data;

            if (coordsInfo.CoordsInfo == null) return;

            foreach (var item in coordsInfo.CoordsInfo)
            {
                CoordsInfo.Add(item);
            }
        }

        IsLoaded = true;
    }
}

public interface ICoordsModel : ILoadableModel
{
    public List<CoordsInfo> CoordsInfo { get; }
}
