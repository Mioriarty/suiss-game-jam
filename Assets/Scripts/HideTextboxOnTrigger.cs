using UnityEngine;

public class HideTextboxOnTrigger : MonoBehaviour
{
    public GameObject textbox;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        textbox.SetActive(false);
    }
}
