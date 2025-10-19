using TMPro;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public TextMeshProUGUI level1HS;
    public TextMeshProUGUI level2HS; 
    public TextMeshProUGUI level3HS;

    void Start()
    {
        
        level1HS.SetText(floatToDisplayable(StaticTracker.GetHighscore(1)));
        level2HS.SetText(floatToDisplayable(StaticTracker.GetHighscore(2)));
        level3HS.SetText(floatToDisplayable(StaticTracker.GetHighscore(3)));
    }
    
    string floatToDisplayable(float t)
    {
        int secs = (int) t % 60;
        int mins = (int)t / 60;
        string secsBufferZero;
        string minsBufferZero;
        if (secs.ToString().Length == 1) { secsBufferZero = "0"; } else { secsBufferZero = ""; }
        if (mins.ToString().Length == 1) { minsBufferZero = "0"; } else { minsBufferZero = ""; }
        return minsBufferZero + mins + ":" + secsBufferZero + secs;
    }
}
