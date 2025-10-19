using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class AdultTriggerController : MonoBehaviour
{
    public GameObject adult;
    public GameObject player;
    public GameObject textBox;
    public GameObject inventoryUI;
    public GameObject directionIndicator;
    public Collider2D firstExhibit;
    public TextboxTextmanager textboxTextmanger;

    public Exhibit secondInterestExhibit;

    private bool checkIfSwapped = false;
    private bool checkForAdultArrival = false;
    private bool checkForAdultArrival2 = false;
    private bool checkForAdultDeparture2 = false;

    void Start()
    {
        textboxTextmanger.SetText("Press w to move forward. With A and D you steer my skateboard. S to break.");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<Collider2D>().enabled = false;
        adult.GetComponent<AdultController>().enabled = true;

        // fix player position in place
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.GetComponent<PlayerController>().enabled = false;
        directionIndicator.SetActive(false);

        // show hint UI
        textBox.SetActive(true);
        textboxTextmanger.SetText("This is my dad. He LOVES books and it looks like he found one...");

        // Call a function after 5 seconds to re-enable player control
        Invoke("ProlongInspectionTime", 0.6f);

    }

    void Update()
    {
        if (checkIfSwapped)
        {
            if (player.GetComponent<PlayerController>().inventoryExhibit.ExhibitName != "apple_crumbs")
            {
                textboxTextmanger.SetText("Great! Now we will bore him to hell.");
                checkIfSwapped = false;
                checkForAdultArrival = true;
            }
        }

        if (checkForAdultArrival)
        {
            if (adult.GetComponent<AdultController>().waitTimer > 0.2f)
            {
                textboxTextmanger.SetText("Onto the next one. Pick up something else to swap it with the next book.");
                checkForAdultArrival = false;
                Invoke("EnableCheckForAdultArrival2", 6.0f);
            }
        }

        if (checkForAdultArrival2)
        {
            if (adult.GetComponent<AdultController>().waitTimer > 0.2f)
            {
                checkForAdultArrival2 = false;
                checkForAdultDeparture2 = true;
            }
        }

        if (checkForAdultDeparture2)
        {
            if (adult.GetComponent<AdultController>().waitTimer < 0.1f)
            {
                adult.GetComponent<AdultController>().speed = 0.0f;
                adult.GetComponent<AdultController>().interests = new Exhibit[]
                {
                    adult.GetComponent<AdultController>().interests[0],
                    secondInterestExhibit
                };
               
                // fix player position in place
                player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                player.GetComponent<PlayerController>().enabled = false;
                directionIndicator.SetActive(false);

                textboxTextmanger.SetText("Oh no, he developed a second interest. Bones... WHHYYYY????");
                Invoke("PrintText11", 6.0f);
                checkForAdultDeparture2 = false;
            }
        }
    }
    
    private void EnableCheckForAdultArrival2()
    {
        checkForAdultArrival2 = true;
    }

    private void ProlongInspectionTime()
    {
        adult.GetComponent<AdultController>().waitTimeAtExhibit = 1000000.0f;
        Invoke("PrintText2", 9.0f);
    }
    private void PrintText2()
    {
        textboxTextmanger.SetText("He seems really interested in it...");
        Invoke("PrintText3", 8.0f);
    }

    private void PrintText3()
    {
        textboxTextmanger.SetText("Omg, this will take ages. We'll never get home...");
        Invoke("PrintText4", 6.0f);
    }

    private void PrintText4()
    {
        textboxTextmanger.SetText("I will makes sure, he will never find another book again in this house. But how?");
        Invoke("PrintText5", 8.0f);
    }

    private void PrintText5()
    {
        textboxTextmanger.SetText("I have an apple crumb with me. Lets swap out the next book he finds with it, by pressing K near it.");
        inventoryUI.SetActive(true);
        Invoke("EnablePlayerControl", 4.0f);
    }

    private void PrintText11()
    {
        textboxTextmanger.SetText("So now, I will have to take care of both.");
        Invoke("PrintText12", 4.0f);
    }

    private void PrintText12()
    {
        textboxTextmanger.SetText("Bore him until boredom bar above is full to finish the level");
        Invoke("StartEndGame", 6.0f);
    }

    private void EnablePlayerControl()
    {
        player.GetComponent<PlayerController>().enabled = true;
        directionIndicator.SetActive(true);
        checkIfSwapped = true;
    }

    private void StartEndGame()
    {
        textBox.SetActive(false);
        adult.GetComponent<AdultController>().speed = 0.7f;
        player.GetComponent<PlayerController>().enabled = true;
        directionIndicator.SetActive(true);
        firstExhibit.enabled = true;

        Invoke("ShowEffectsHint", 20.0f);
    }


    private void ShowEffectsHint()
    {
        textBox.SetActive(true);
        textboxTextmanger.SetText("Oh, one more thing: Some exhibits give special effects when in inventory. Good Luck!");
        Invoke("HideTextBoxLastTime", 12.0f);
    }

    private void HideTextBoxLastTime()
    {
        textBox.SetActive(false);
    }
}
