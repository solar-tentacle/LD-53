using System.Collections;
using UnityEngine.SceneManagement;

public class GameFlowService : IService, IInject
{
    private AssetsCollection _assetsCollection;
    private UIService _uiService;
    public static uint LevelIndex;
    public static int DieCount;

    void IInject.Inject()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _uiService = Services.Get<UIService>();
    }

    public void GoToNextLevel()
    {
        LevelIndex++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public IEnumerator CompleteLevel()
    {
        if (_assetsCollection.IsLastLevel(LevelIndex))
        {
            _uiService.UICanvas.AllLevelsCompletedWindow.Show();
            yield return _uiService.UICanvas.AllLevelsCompletedWindow.WaitForClose();
            GoToNextLevel();
        }
        else
        {
            GoToNextLevel();
            yield break;
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseGame(string reasonText)
    {
        DieCount++;
        
        _uiService.UICanvas.UILoseWindow.SetContent(reasonText);
        _uiService.UICanvas.UILoseWindow.Show();
    }
}