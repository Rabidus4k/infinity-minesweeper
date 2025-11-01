using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class UIRestartButton : UICustomButton
{
    private IGameViewModel _gameViewModel;
    private ISaveService _saveService;

    [Inject]
    private void Construct(IGameViewModel gameViewModel, ISaveService saveService)
    {
        _gameViewModel = gameViewModel;
        _saveService = saveService;
    }

    protected override void HandleClick()
    {
        SceneManager.LoadScene("GameScene");
    }
}
