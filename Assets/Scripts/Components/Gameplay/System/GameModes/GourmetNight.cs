using UnityEngine;

public class GourmetNight : LunchRush
{
    [SerializeField] float IncrementTimeInSeconds = 50f; 
    public override void Init()
    {
        base.Init();
        _startTime = 300f;
    }

    public void TimeMultiplier()
    {
        CurrentTimer += IncrementTimeInSeconds;
    }
}
