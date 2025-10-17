using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    public GameObject target;
    public String[] interests;
    public float speed = 1.0f;

    public List<Vector3> path;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && path != null && path.Count > 0)
        {
            Vector3 targetPosition = path[0];
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                path.RemoveAt(0);
            }

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

            path = AStarManager.instance.GeneratePath(transform.position, target.transform.position);

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
}
