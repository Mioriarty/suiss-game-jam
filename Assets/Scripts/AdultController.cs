using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AdultController : MonoBehaviour
{
    public GameObject target;
    public String[] interests;
    public float speed = 1.0f;
    public float safetyDistance = 1.0f;
    private bool reachedTarget = false;
    private bool isWaiting = false;
    public float waitTimeAtExhibit = 2.0f;
    private float waitTimer = 0.0f;

    public List<Vector3> path;

    public float MaxBoredom, Boredom;
    [SerializeField] BoredomBarController boredomBarController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boredomBarController.SetMaxBoredom(MaxBoredom);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Target: " + (target != null ? target.GetComponent<ExhibitController>().exhibit.Interest : "None") + "\n Distance to Target: " + (target != null ? Vector3.Distance(transform.position, target.transform.position).ToString() : "N/A"));
        if (target != null && path != null && path.Count > 0)
        {
            Debug.Log("Moving towards target exhibit: " + target.GetComponent<ExhibitController>().exhibit.Interest);
            Vector3 targetPosition = path[0];
            if (!isWaiting)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            }
            

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                path.RemoveAt(0);
            }

            if (Vector3.Distance(transform.position, target.transform.position) < safetyDistance)
            {
                Debug.Log("Reached target exhibit: " + target.GetComponent<ExhibitController>().exhibit.Interest);
                reachedTarget = true;
                if (Array.Exists(interests, interest => interest == target.GetComponent<ExhibitController>().exhibit.Interest))
                {
                    Debug.Log("Target is of interest.");
                    SetBoredom(-10f);
                }
                else
                {
                    Debug.Log("Target is not of interest.");
                    SetBoredom(10f);
                }
            }
        }
        


        if (target == null || reachedTarget)
        {
            // adult waits for a moment before selecting a new target
            isWaiting = true;
            waitTimer += Time.deltaTime;
            if (waitTimer < waitTimeAtExhibit)
            {
                return;
            }
            waitTimer = 0.0f;
            isWaiting = false;


            Debug.Log("Selecting new target for adult.");
            ExhibitController[] exhibits = FindObjectsByType<ExhibitController>(FindObjectsSortMode.None);

            // filter exhibits by interests
            exhibits = Array.FindAll(exhibits, exhibit => Array.Exists(interests, interest => interest == exhibit.exhibit.Interest));

            // filter out current target
            if (target != null)
            {
                exhibits = Array.FindAll(exhibits, exhibit => exhibit.gameObject != target);
            }

            

            // select a random exhibit
            ExhibitController randomExhibit = exhibits[UnityEngine.Random.Range(0, exhibits.Length)];
            target = randomExhibit.gameObject;

            path = AStarManager.instance.GeneratePath(transform.position, target.transform.position);
            reachedTarget = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            // Prioritize this drawing, put high z value


            Gizmos.color = Color.red;

            // Draw path as line
            for (int i = 0; i < path.Count - 1; i++)
            {
                // Prioritize this drawing, put high z value
                Gizmos.DrawLine(path[i] + Vector3.forward * 0.1f, path[i + 1] + Vector3.forward * 0.1f);
            }
        }
    }

    public void SetBoredom(float boredomChange)
    {
        Boredom += boredomChange;
        Boredom = Mathf.Clamp(Boredom, 0, MaxBoredom);
        boredomBarController.SetBoredom(Boredom);
    }
}
