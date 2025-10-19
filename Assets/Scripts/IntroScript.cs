using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class IntroScript : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public Image introImage;
    public Sprite handsEmpty;
    public Sprite handsEnvelope;
    public Sprite handsTicket;
    public int textProgression = 0;
    public int currText = -1;
    public string[] introTextArray;
    public Sprite[] introSpriteArray;
    public GameObject LoadManager;

    void Start()
    {
        introText.SetText("I am so excited for my birthday present!");
        introImage.sprite = handsEmpty;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow) || (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
        {
            textProgression += 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame))
        {
            textProgression -= 1;
        }

        if (textProgression >= introTextArray.Length)
        {
            LoadManager.GetComponent<LoadController>().LoadScene("Level 0");
        }
        else if (textProgression != currText)
        {
            introText.SetText(introTextArray[textProgression]);
            introImage.sprite = introSpriteArray[textProgression];
            currText = textProgression;
        }
    }
}
