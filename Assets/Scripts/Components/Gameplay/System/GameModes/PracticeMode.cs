using UnityEngine;
public class PracticeMode : LunchRush
{
    // [SerializeField] CanvasGroup _canvasPrefab;
    // GameObject _canvasObject;
    [SerializeField] int _serveGoal = 5;
    private int _currentServes = 0;
    private bool _practiceModeEnded = false;

    private void Update()
    {
        if (_currentServes == _serveGoal)
        {
            End();
        }
    }

    public void AddServe()
    {
        _currentServes++;
    }

    public override void Init()
    {

    }

    public override void End()
    {
        if (!_practiceModeEnded)
        {
            _practiceModeEnded = true;
            GameplayController.Instance.EndGame();
        }
    }
}
