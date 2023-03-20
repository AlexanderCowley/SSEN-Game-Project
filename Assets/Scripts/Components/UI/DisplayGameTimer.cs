using UnityEngine;
using TMPro;
public class DisplayGameTimer : MonoBehaviour
{
    TextMeshProUGUI _text;
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        LunchRush.Instance.OnTimerUpdate += UpdateText;
    }

    void OnDisable()
    {
        LunchRush.Instance.OnTimerUpdate -= UpdateText;
    }

    void UpdateText()
    {
        _text.SetText($"{LunchRush.Instance.Minutes}:{LunchRush.Instance.Seconds.ToString("00")}");
    }
}
