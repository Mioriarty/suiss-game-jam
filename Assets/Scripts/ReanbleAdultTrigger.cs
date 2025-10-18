using UnityEngine;

public class ReanbleParentTrigger : MonoBehaviour
{
    public AdultController adultController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<Collider2D>().enabled = false;
        adultController.waitTimeAtExhibit = 4.0f;
    }

}
