using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject loadManager;
    public float failureSeconds;
    public bool isLevel;
    private float startTime;
    public int levelNumber = -1;

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
        loadManager.GetComponent<LoadController>().LoadScene("GameResultScreen");
    }
}
