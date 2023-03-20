using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _titlePage, _gameModesPage, _techniquesPage, _settingsPage, _controlsPage, _creditsPage;
    [SerializeField] private string _tutorialSceneName, _timedSceneName, _survivalSceneName;

    private MenuStates.MenuState _currentMenuState;
    private string _targetSceneName;

    public MenuStates.MenuState CurrentMenuState { get { return _currentMenuState; } }

    private void Update()
    {
        UpdateMenu();
    }

    private void UpdateMenu()
    {
        _titlePage.SetActive(_currentMenuState == MenuStates.MenuState.Title);
        _gameModesPage.SetActive(_currentMenuState == MenuStates.MenuState.GameModes);
        _techniquesPage.SetActive(_currentMenuState == MenuStates.MenuState.Techniques);
        _settingsPage.SetActive(_currentMenuState == MenuStates.MenuState.Settings);
        _controlsPage.SetActive(_currentMenuState == MenuStates.MenuState.Controls);
        _creditsPage.SetActive(_currentMenuState == MenuStates.MenuState.Credits);
    }

    public void ChangeMenuState(MenuStates.MenuState newState)
    {
        _currentMenuState = newState;
    }

    public void ChangeForbiddenTechnique(int id)
    {
        PlayerPrefs.SetInt("ForbiddenTechniqueID", id);
    }

    public void ChangeTargetScene(string sceneName)
    {
        _targetSceneName = sceneName;
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene(_targetSceneName);
        Debug.Log($"Going to Game Scene: {_targetSceneName} with Forbidden Technique: {PlayerPrefs.GetInt("ForbiddenTechniqueID")}");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
