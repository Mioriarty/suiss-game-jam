using UnityEngine;

public class PlaySomeMessage : MonoBehaviour
{

    public TextboxTextmanager textboxTextmanager;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Adult"))
        {
            GetComponent<Collider2D>().enabled = false;
            textboxTextmanager.SetText("Onto the next book!! He will never find something interesting around here.");
        }
    }
}
