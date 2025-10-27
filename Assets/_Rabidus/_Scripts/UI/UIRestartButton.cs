using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRestartButton : UICustomButton
{
    protected override void HandleClick()
    {
        SceneManager.LoadScene("GameScene");
    }
}
