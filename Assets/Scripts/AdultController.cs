using System;
using System.Collections.Generic;
using UnityEngine;

public class AdultController : MonoBehaviour
{
    public Animator adultAnimator;
    public GameObject target;
    public Exhibit[] interests;
    private int currentInterestIndex = 0;
    public float speed = 1.0f;
    public float safetyDistance = 1.0f;
    public GameObject desiredObject;
    private SpriteRenderer desiredObjectRenderer;
    
    private bool reachedTarget = false;
    private bool isWaiting = false;
    public float waitTimeAtExhibit = 2.0f;
    private float waitTimer = 0.0f;

    public List<Vector3> path;

    public float MaxBoredom, Boredom;
    public List<GameObject> nextTargets;

    [SerializeField] private Canvas uiCanvas;                  
    [SerializeField] private BoredomBarController boredomBarPrefab;
    [SerializeField] private RectTransform boredomBarContainer;
    [SerializeField] private float boredomBarSpacing = 0.5f;
    [SerializeField] BoredomBarController boredomBarController;
    [SerializeField] private Color barColor = Color.yellow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desiredObjectRenderer = desiredObject.GetComponent<SpriteRenderer>();
        CircleInterestingObject();
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        // find the sprite renderer named "AdultSprite"
        SpriteRenderer spriteRenderer = null;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr.gameObject.name == "AdultSprite")
            {
                spriteRenderer = sr;
                break;
            }
        }

        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            string spriteName = spriteRenderer.sprite.name;
            // split by underscore 
            string id = spriteName.Split('n')[1].Split(' ')[0];

            int.TryParse(id, out int exhibitId);

            if (exhibitId == 11)
            {
                ColorUtility.TryParseHtmlString("#1eda02", out barColor);
            }
            else if (exhibitId == 21)
            {
                ColorUtility.TryParseHtmlString("#fc4e51", out barColor);
            }
            else if (exhibitId == 31)
            {
                ColorUtility.TryParseHtmlString("#614dfd", out barColor);
            }

            Debug.Log("Adult sprite ID: " + id + " Color: " + barColor);

        }
        EnsureBoredomBar();
        if (boredomBarController != null)
        {   

            boredomBarController.SetMaxBoredom(MaxBoredom);
            boredomBarController.SetFillColor(barColor);
        }
    }

    bool IsInterestedIn(Exhibit exhibit)
    {
        foreach (var interest in interests)
        {
            if (interest == exhibit)
                return true;
        }
        return false;
    }
    
    void EnsureBoredomBar()
    {
        if (uiCanvas == null)
            uiCanvas = FindFirstObjectByType<Canvas>();
        if (uiCanvas == null || boredomBarPrefab == null)
            return;

        // make sure container exists
        if (boredomBarContainer == null)
        {
            var found = uiCanvas.transform.Find("BoredomBarContainer");
            if (found != null)
                boredomBarContainer = found.GetComponent<RectTransform>();
        }
        if (boredomBarContainer == null)
        {
            // offset container by some pixels from top
            var go = new GameObject("BoredomBarContainer", typeof(RectTransform));
            boredomBarContainer = go.GetComponent<RectTransform>();
            boredomBarContainer.SetParent(uiCanvas.transform, false);
            boredomBarContainer.anchorMin = new Vector2(0.5f, 1f);
            boredomBarContainer.anchorMax = new Vector2(0.5f, 1f);
            boredomBarContainer.pivot    = new Vector2(0.5f, 1f);
            boredomBarContainer.anchoredPosition = new Vector2(0f, -2f);
        }

        // number of existing bars
        int index = boredomBarContainer.childCount;

        // create new bar
        boredomBarController = Instantiate(boredomBarPrefab, boredomBarContainer);

        // position bar in container
        RectTransform barRect = boredomBarController.GetComponent<RectTransform>();
        barRect.anchorMin = new Vector2(0.5f, 1f);
        barRect.anchorMax = new Vector2(0.5f, 1f);
        barRect.pivot     = new Vector2(0.5f, 1f);

        float rowHeight = barRect.sizeDelta.y;
        barRect.anchoredPosition = new Vector2(0f, -index * (rowHeight + boredomBarSpacing));
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && path != null && path.Count > 0)
        {
            Vector3 targetPosition = path[0];
            if (!isWaiting)
            {
                if(targetPosition.x > transform.position.x)
                    transform.localScale = new Vector3(1, 1, 1);
                else if(targetPosition.x < transform.position.x)
                    transform.localScale = new Vector3(-1, 1, 1);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            }
            

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                path.RemoveAt(0);
            }

            if (Vector3.Distance(transform.position, target.transform.position) < safetyDistance && !reachedTarget)
            {
                reachedTarget = true;
                if (IsInterestedIn (target.GetComponent<ExhibitController>().exhibit))
                {
                    SetBoredom(-10f);
                    adultAnimator.SetBool("isHappy", true);
                }
                else
                {
                    SetBoredom(10f);
                    adultAnimator.SetBool("isBored", true);
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

            // got back to walking state
            adultAnimator.SetBool("isHappy", false);
            adultAnimator.SetBool("isBored", false);

            ExhibitController targetExhibit;
            if (nextTargets!= null && nextTargets.Count > 0)
            {
                target = nextTargets[0];
                nextTargets.RemoveAt(0);
                Debug.Log("Selecting predefined target for adult. remaining targets: " + nextTargets.Count);
                targetExhibit = target.GetComponent<ExhibitController>();
            }            
            else
            {
                Debug.Log("Selecting new target for adult.");
                ExhibitController[] exhibits = FindObjectsByType<ExhibitController>(FindObjectsSortMode.None);

                // filter exhibits by interests
                exhibits = Array.FindAll(exhibits,
                    exhibit => IsInterestedIn(exhibit.exhibit));

                // filter out current target
                if (target != null)
                {
                    exhibits = Array.FindAll(exhibits, exhibit => exhibit.gameObject != target);
                }

                Debug.Log("Found " + exhibits.Length + " exhibits of interest.");

                // select a random exhibit

                if (exhibits.Length == 0)
                {
                    Debug.LogWarning("No exhibits found for adult's interests.");
                    return;
                }
                targetExhibit = exhibits[UnityEngine.Random.Range(0, exhibits.Length)];
                target = targetExhibit.gameObject;
            }
            
            Debug.Log("New target selected: " + target.transform.position);
            Debug.Log("Current position: " + transform.position);
            path = AStarManager.instance.GeneratePath(transform.position, target.transform.position);
            reachedTarget = false;
        }
    }

    void CircleInterestingObject()
    {
        if (interests.Length == 0)
            return;
        desiredObjectRenderer.sprite = this.interests[currentInterestIndex].Image;
        currentInterestIndex = (currentInterestIndex + 1) % interests.Length;
        Invoke("CircleInterestingObject", 1.0f);
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

    private void OnDestroy()
    {
        // Eigene UI-Bar aufräumen, wenn dieses Adult zerstört wird
        if (boredomBarController != null)
        {
            Destroy(boredomBarController.gameObject);
        }
    }
}
