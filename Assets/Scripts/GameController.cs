using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public LoadController loadController;
    public float failureSeconds;
    public bool isLevel;
    private float startTime;
    public int levelNumber = -1;

    private List<AdultController> notBoredAdults;
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (isLevel && Time.time > startTime + failureSeconds)
        {
            EndLevelByTimeElapsed();
        }
    }

    void EndLevelByTimeElapsed()
    {
        StaticTracker.SetGameResult(levelNumber, StaticTracker.GameEndType.TimeElapsed, Time.time);
        loadController.LoadScene("GameResultScreen");
    }
    
    public void EndLevelByBoredom()
    {
        StaticTracker.SetGameResult(levelNumber, StaticTracker.GameEndType.Success, Time.time);
        loadController.LoadScene("GameResultScreen");
    }
    
    public void AdultBecameBored(AdultController adult)
    {
        notBoredAdults.Remove(adult);
        if (notBoredAdults.Count == 0)
        {
            EndLevelByBoredom();
        }
    }
    
    public void RegisterAdult(AdultController adult)
    {
        if (notBoredAdults == null) notBoredAdults = new List<AdultController>();
        notBoredAdults.Add(adult);
    }
    
}
