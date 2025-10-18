using System;
using TMPro;
using UnityEngine;

public class FailureScreenManager : MonoBehaviour
{
    public GameObject loadManager;
    public TextMeshProUGUI failureDesc; 

    void Start()
    {
        if (StaticTracker.GetGameOverParams() == null) { loadManager.GetComponent<LoadController>().LoadScene("MainMenu"); }
        StaticTracker.GameOverParams gop = StaticTracker.GetGameOverParams();
        failureDesc.SetText($"Your {gop.Level}-th birthday bored you to death. At least your parents had a fun {gop.FailureTime} hours! ");
    }
}
