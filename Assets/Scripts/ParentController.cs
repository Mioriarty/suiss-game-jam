using System;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    public GameObject target;
    public String[] interests;
    public float speed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        
            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                if (Array.Exists(interests, interest => interest == target.GetComponent<ExhibitController>().exhibit.Interest))
                {
                    Debug.Log("Reached target exhibit of interest: " + target.GetComponent<ExhibitController>().exhibit.Interest);
                }
                else
                {
                    Debug.Log("Reached target exhibit, but not of interest: " + target.GetComponent<ExhibitController>().exhibit.Interest);
                }
            }
        }


        if (target == null || Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            ExhibitController[] exhibits = FindObjectsByType<ExhibitController>(FindObjectsSortMode.None);

            // filter exhibits by interests
            exhibits = Array.FindAll(exhibits, exhibit => Array.Exists(interests, interest => interest == exhibit.exhibit.Interest));

            // select a random exhibit
            ExhibitController randomExhibit = exhibits[UnityEngine.Random.Range(0, exhibits.Length)];
            target = randomExhibit.gameObject;

        }
    }
}
