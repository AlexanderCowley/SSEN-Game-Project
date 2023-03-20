using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStates : MonoBehaviour
{
    [System.Serializable]
    public enum MenuState
    {
        Title,
        GameModes,
        Techniques,
        Settings,
        Controls,
        Credits
    }
    [SerializeField] private MenuState _destination;
    private MainMenuController _mainMenuController;

    private void Awake()
    {
        _mainMenuController = FindObjectOfType<MainMenuController>();
    }

    public void GoToDestination()
    {
        _mainMenuController.ChangeMenuState(_destination);
    }
}
