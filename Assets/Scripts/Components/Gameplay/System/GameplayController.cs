using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance { get; private set; }

    [SerializeField] private GameObject _playerObject;
    [SerializeField] private Recipe[] _recipes;
    [SerializeField] private int _forbiddenTechniqueID;
    [SerializeField] private TextMeshProUGUI _evaluationText;
    [SerializeField] private Animator _evaluationTextPanelAnimator;
    [SerializeField] private GameObject _resultsPanel, _pausePanel, _settingsPanel;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _resultsPointsEarnedText, _resultsDishesServedText, _resultsPercentagePerfectText, _resultsSupersUsedText;
    [SerializeField] private AudioClip _endingClip;

    // Tracked Info
    private int _pointsEarned;
    private int _dishesServed, _perfectDishesServed;
    private int _supersUsed;
    private List<Item> _allIngredients = new List<Item>();
    private StoveTopStation[] _allStoves;
    private AudioSource _audioSource;

    private bool _paused = false;
    private bool _ended = false;

    public GameObject PlayerObject { get { return _playerObject; } }
    public Recipe[] Recipes { get { return _recipes; } }
    public int ForbiddenTechniqueID { get { return _forbiddenTechniqueID; } }
    public List<Item> AllIngredients { get { return _allIngredients; } }
    public StoveTopStation[] AllStoves { get { return _allStoves; } }
    public bool Paused { get { return _paused; } }
    public AudioSource SFXAudioSource { get { return _audioSource; } }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Get persistent data from Main Menu
        _forbiddenTechniqueID = PlayerPrefs.GetInt("ForbiddenTechniqueID");
        _audioSource = GetComponent<AudioSource>();
        GetStoves();
        InitializeTrackedValues();
        ShowSettings(false);
        OpenPauseMenu(false);
        DisplayResults();
    }

    private void GetStoves()
    {
        _allStoves = FindObjectsOfType<StoveTopStation>();
    }

    public void TrackIngredient(Item item, bool remove)
    {
        if (item.Type != Item.ItemType.Plate)
        {
            if (remove)
            {
                _allIngredients.Remove(item);
            }
            else
            {
                _allIngredients.Add(item);
            }
        }
    }

    private void InitializeTrackedValues()
    {
        _pointsEarned = 0;
        _pointsText.text = $"{_pointsEarned} Points";
        _dishesServed = 0;
        _supersUsed = 0;
    }

    public void AddPoints(int value)
    {
        _pointsEarned += value;
        _pointsText.text = $"{_pointsEarned} Points";

        _dishesServed++;
        if (value == 10)
        {
            _perfectDishesServed++;
        }
    }

    public void UsedSuper()
    {
        _supersUsed++;
    }

    public void DisplayEvaluationText(string textToDisplay)
    {
        _evaluationText.text = textToDisplay;
        _evaluationTextPanelAnimator.Play("qualityEvaluationPresentation");
    }

    private void DisplayResults()
    {
        _resultsPointsEarnedText.text = $"<b>Points Earned:</b> {_pointsEarned}";
        _resultsDishesServedText.text = $"<b>Dishes Served:</b> {_dishesServed}";
        _resultsPercentagePerfectText.text = $"<b>Perfect Dishes:</b> {_perfectDishesServed}";
        _resultsSupersUsedText.text = $"<b>Forbidden Techniques Used:</b> {_supersUsed}";
        _resultsPanel.gameObject.SetActive(_ended);
    }

    public void OpenPauseMenu(bool pause)
    {
        if (_settingsPanel.activeInHierarchy)
        {
            ShowSettings(false);
        }
        else
        {
            if (!_ended)
            {
                Time.timeScale = pause ? 0 : 1;
                _pausePanel.SetActive(pause);
                _settingsPanel.SetActive(false);
                _paused = pause;
            }
        }
    }

    public void ShowSettings(bool toggle)
    {
        if (_paused)
        {
            _pausePanel.SetActive(!toggle);
            _settingsPanel.SetActive(toggle);
        }
        else
        {
            _settingsPanel.SetActive(false);
        }
    }

    public void EndGame()
    {
        _ended = true;
        Time.timeScale = 0;
        DisplayResults();
        _audioSource.PlayOneShot(_endingClip);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
