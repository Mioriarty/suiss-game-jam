using System;
using UnityEngine;

public class AdultTriggerController : MonoBehaviour
{
    public GameObject adult;
    private void OnTriggerEnter2D(Collider2D other)
    {
        adult.GetComponent<AdultController>().enabled = true;
    }
}
