using System;
using UnityEngine;

public class VisitorController : MonoBehaviour
{
    private Vector2 _originalPosition;
    public float moveSpeed;
    public float distanceThreshold;
    public float distance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _originalPosition = transform.position;
    }
    

    // Update is called once per frame
    void Update()
    {
        // keep distance to all objects with tag "Adult"
        GameObject[] adults = GameObject.FindGameObjectsWithTag("Adult");
        bool isNearAdult = false;
        foreach (GameObject adult in adults)
        {
            distance = Vector2.Distance(transform.position, adult.transform.position);
            if (distance < distanceThreshold)
            {
                isNearAdult = true;
                Vector2 directionAway = (transform.position - adult.transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position,
                    (Vector2)transform.position + directionAway, moveSpeed * Time.deltaTime);
            }
        }
        if (!isNearAdult && Vector2.Distance(transform.position, _originalPosition) > 0.1f)
        {
            Vector2 directionToOriginal = (_originalPosition - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position,
                (Vector2)transform.position + directionToOriginal, moveSpeed * Time.deltaTime);
        }
    }
}
