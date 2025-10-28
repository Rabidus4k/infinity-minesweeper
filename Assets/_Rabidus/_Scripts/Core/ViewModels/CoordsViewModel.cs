using System.Collections.Generic;

public class CoordsViewModel : ICoordsViewModel
{
    public ReactiveProperty<List<CoordsInfo>> CoordsInfo { get; private set; } = new ReactiveProperty<List<CoordsInfo>>();

    private ICoordsModel _model;

    public CoordsViewModel(ICoordsModel model)
    {
        _model = model;
        CoordsInfo.Value = _model.CoordsInfo;
    }

    public void AddCoordsInfo(CoordsInfo coordsInfo)
    {
        CoordsInfo.Value.Add(coordsInfo);
    }

    public void RemoveCoordsInfo(CoordsInfo coordsInfo)
    {
        CoordsInfo.Value.Remove(coordsInfo);
    }
}

public interface ICoordsViewModel 
{
    public ReactiveProperty<List<CoordsInfo>> CoordsInfo { get; }
    void AddCoordsInfo(CoordsInfo coordsInfo);
    void RemoveCoordsInfo(CoordsInfo coordsInfo);
}
