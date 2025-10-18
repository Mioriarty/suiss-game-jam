using System;
using UnityEngine;

public class AdultTriggerController : MonoBehaviour
{
    public GameObject adult;
    public GameObject player;
    public GameObject hintUI;
    private void OnTriggerEnter2D(Collider2D other)
    {
        adult.GetComponent<AdultController>().enabled = true;
        
        // fix player position in place
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.GetComponent<PlayerController>().enabled = false;
        
        // show hint UI
        hintUI.SetActive(true);
    }
}
