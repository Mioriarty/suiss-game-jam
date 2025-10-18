using System;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;

public class GameEndScreenManager : MonoBehaviour
{
    public GameObject loadManager;
    public TextMeshProUGUI gameResultAnnounce;
    public TextMeshProUGUI gameDesc;
    StaticTracker.GameResult res;

    void Start()
    {
        if (StaticTracker.GetGameResult() == null)
        {
            loadManager.GetComponent<LoadController>().LoadScene("MainMenu");
            Debug.LogError("Something has gone wrong: No GameResult found.");
        }
        res = StaticTracker.GetGameResult();
        switch (res.GameEnd)
        {
            case StaticTracker.GameEndType.TimeElapsed: TimeElapsed(); break;
            case StaticTracker.GameEndType.Success: Success(); break;
            default: break;
        }

    }

    void TimeElapsed()
    {
        gameResultAnnounce.SetText("You failed.");
        gameDesc.SetText($"Your {res.Level}-th birthday bored you to death. At least your parents had a fun {res.EndTime} hours! ");
    }
    
    void Success()
    {
        gameResultAnnounce.SetText("You succeeded!");
        gameDesc.SetText($"Your {res.Level}-th birthday turned into a fun minigame against your parents. And you only had to spend {res.EndTime} hours at the museum! ");
    }
}
