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

    String GetOrdinal(int number)
    {
        if (number % 10 == 1 && number % 100 != 11) { return $"{number}st"; }
        else if (number % 10 == 2 && number % 100 != 12) { return $"{number}nd"; }
        else if (number % 10 == 3 && number % 100 != 13) { return $"{number}rd"; }
        else { return $"{number}th"; }
    }
    void TimeElapsed()
    {
        gameResultAnnounce.SetText("You failed.");
        gameDesc.SetText($"Your {GetOrdinal(res.Level)} birthday bored you to death. At least your parents had a fun {StaticTracker.floatToDisplayable(res.EndTime)} minutes! ");
    }
    
    void Success()
    {
        gameResultAnnounce.SetText("You succeeded!");
        if (res.Level == 0)
        {
            gameDesc.SetText("You completed the tutorial level! Congratulations! ");
            return;
        }
        gameDesc.SetText($"Your {GetOrdinal(res.Level+6)} birthday turned into a fun minigame against your parents. And you only had to spend {StaticTracker.floatToDisplayable(res.EndTime)} minutes at the museum! ");
    }
}
