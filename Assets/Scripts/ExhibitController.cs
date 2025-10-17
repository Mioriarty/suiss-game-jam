using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]


// Simple controller that applies an Exhibit ScriptableObject to UI components.
public class ExhibitController : MonoBehaviour
{
    [SerializeField] private Exhibit exhibit;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Apply();
    }

    public void Apply()
    {
        if (exhibit == null)
            return;

        // Image
        if (spriteRenderer != null)
            spriteRenderer.sprite = exhibit.Image;
    }

    public void SetExhibit(Exhibit newExhibit)
    {
        exhibit = newExhibit;
        Apply();
    }
}
