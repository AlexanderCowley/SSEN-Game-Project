using System;
using UnityEngine;
public class LunchRush : MonoBehaviour
{
    public static LunchRush Instance { get; private set; }

    private bool _lunchRushEnded = false;

    //[SerializeField] CanvasGroup _canvasGroupPrefab;
    //GameObject gameModeCanvasObject;

    public Action OnTimerStarted;
    public Action OnTimerFinshed;
    public Action OnTimerUpdate;

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioClip _alternateBGM;
    [SerializeField] protected float _startTime = 5f;
    private bool _minLeft = false;
    float _currentTime = 0f;
    public float Seconds { get; private set; }
    public float Minutes { get; private set; }
    public string EndGameMessage { get; private set; } = String.Empty;

    //Timer value that invokes an event every tick to update the clock
    public float CurrentTimer
    {
        get
        {
            return _currentTime;
        }
        protected set
        {
            OnTimerUpdate?.Invoke();
            _currentTime = value;
        }
    }

    bool _timerActive = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        //Init canvases
        //gameModeCanvasObject = Instantiate(_canvasGroupPrefab.gameObject);
        CurrentTimer = _startTime;
        _timerActive = true;
    }

    public virtual void End()
    {
        _timerActive = false;
        OnTimerFinshed?.Invoke();
        // GameplayController.Instance.PlayerObject.GetComponent<Movement>().enabled = false;
        if (!_lunchRushEnded)
        {
            _lunchRushEnded = true;
            GameplayController.Instance.EndGame();
        }
    }

    void UpdateTimer()
    {
        CurrentTimer -= Time.deltaTime;
        Minutes = Mathf.FloorToInt(CurrentTimer / 60);
        Seconds = Mathf.FloorToInt(CurrentTimer % 60);
        if ((_currentTime < 60) && !_minLeft)
        {
            _minLeft = true;
            _bgmSource.clip = _alternateBGM;
            _bgmSource.Play();
        }
        if (_currentTime < 1)
            End();
    }

    void Update()
    {
        if(_timerActive) 
            UpdateTimer();
    }
}
