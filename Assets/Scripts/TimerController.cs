using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float realtimeElapsed = 0;
    private int secs = 0;
    private string secsBufferZero = "0";
    private int mins = 0;
    private string minsBufferZero = "0"; 

    void Start() {
        timerText.SetText("00:00");
    }

    void Update()
    {
        realtimeElapsed += Time.deltaTime;
        secs = (int) realtimeElapsed % 60;
        mins = (int) realtimeElapsed / 60;
        if (secs.ToString().Length == 1) { secsBufferZero = "0"; } else { secsBufferZero = ""; }
        if (mins.ToString().Length == 1) { minsBufferZero = "0"; } else { minsBufferZero = ""; }
        timerText.SetText(minsBufferZero + mins + ":" + secsBufferZero + secs);
    }
}
