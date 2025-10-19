using TMPro;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public TextMeshProUGUI level1HS;
    public TextMeshProUGUI level2HS; 
    public TextMeshProUGUI level3HS;

    void Start()
    {
        
        level1HS.SetText(StaticTracker.floatToDisplayable(StaticTracker.GetHighscore(1)));
        level2HS.SetText(StaticTracker.floatToDisplayable(StaticTracker.GetHighscore(2)));
        level3HS.SetText(StaticTracker.floatToDisplayable(StaticTracker.GetHighscore(3)));
    }
    
}
