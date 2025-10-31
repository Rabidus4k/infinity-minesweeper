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
        RestartGameAsync().Forget();
    }

    private async UniTask RestartGameAsync()
    {
        _gameViewModel.Cells.Value.Clear();

        await _saveService.SaveGame();

        SceneManager.LoadScene("GameScene");
    }
}
