using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class ResultScreenToggle : MonoBehaviour
{
    CanvasGroup _canvasGroup;
    [Header("Settings")]
    //Determines whether a canvas should be active on Start
    [SerializeField] bool ActiveOnStart;
    public bool IsActive { get; private set; } = false;
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        LunchRush.Instance.OnTimerFinshed += ToggleCanvas;

        if (ActiveOnStart)
            ToggleCanvas();
    }

    void OnDisable() => LunchRush.Instance.OnTimerFinshed -= ToggleCanvas;

    void ToggleCanvas()
    {
        IsActive = !IsActive;
        _canvasGroup.alpha = IsActive ? 1 : 0;
    }
}
