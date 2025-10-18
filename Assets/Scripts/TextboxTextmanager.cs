using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextboxTextmanager : MonoBehaviour
{

    public float letterDelay = 0.05f;
    public TextMeshProUGUI textBoxText;

    private string newText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        textBoxText = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string newText)
    {
        this.newText = newText;
        StopAllCoroutines();
        textBoxText.text = "";

        StartCoroutine(TypeText());

        IEnumerator TypeText()
        {
            for (int i = 0; i < this.newText.Length; i++)
            {
                textBoxText.text += this.newText[i];
                yield return new WaitForSeconds(letterDelay);
            }
        }
    }

}
