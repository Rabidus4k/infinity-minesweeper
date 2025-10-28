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
}

public interface ICoordsViewModel 
{
    public ReactiveProperty<List<CoordsInfo>> CoordsInfo { get; }
}
