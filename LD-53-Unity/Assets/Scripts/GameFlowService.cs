﻿using UnityEngine.SceneManagement;

public class GameFlowService : IService, IInject
{
    private AssetsCollection _assetsCollection;
    private UIService _uiService;
    public static uint LevelIndex;

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
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        throw new System.NotImplementedException();
    }
}