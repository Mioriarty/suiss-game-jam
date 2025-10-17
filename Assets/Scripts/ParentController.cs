using UnityEngine;

public class ParentController : MonoBehaviour
{
    public ExhibitController target;
    public float speed = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 targetPosition = target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        }


        if (target == null || Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            ExhibitController[] exhibits = FindObjectsByType<ExhibitController>(FindObjectsSortMode.None);

            // select a random exhibit
            ExhibitController randomExhibit = exhibits[Random.Range(0, exhibits.Length)];
            target = randomExhibit;

        }
    }
}
