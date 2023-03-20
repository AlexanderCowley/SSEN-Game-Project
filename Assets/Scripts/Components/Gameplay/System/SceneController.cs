using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(this);
    }

    public void GoToMainMenu() => SceneManager.LoadScene(0);

    public void GoToLunchRush() => SceneManager.LoadScene(1);

    public void GoToPracticeMode() => SceneManager.LoadScene(2);

    public void GoToHardMode() => SceneManager.LoadScene(3);

}
