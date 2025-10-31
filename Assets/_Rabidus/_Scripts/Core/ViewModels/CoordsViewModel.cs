using System.Collections.Generic;

public class CoordsViewModel : ICoordsViewModel
{
    public ReactiveProperty<bool> IsLoaded { get; private set; } = new ReactiveProperty<bool>();
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

    public void LoadData(object data)
    {
        _model.LoadData(data);
        IsLoaded.Value = _model.IsLoaded;
    }
}

public interface ICoordsViewModel : ILoadableViewModel
{
    public ReactiveProperty<List<CoordsInfo>> CoordsInfo { get; }
    void AddCoordsInfo(CoordsInfo coordsInfo);
    void RemoveCoordsInfo(CoordsInfo coordsInfo);
}
