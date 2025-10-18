using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoredomBarController : MonoBehaviour
{
    public float Boredom, MaxBoredom, Width, Height;

    public Color FillColor = Color.yellow;
    [SerializeField] private RectTransform boredomBarRect;

    public void SetFillColor(Color color)
    {
        FillColor = color;
        Image img = boredomBarRect.GetComponent<Image>();
        if (img != null)
        {
            img.color = FillColor;
        }
    }

    public void SetMaxBoredom(float max)
    {
        MaxBoredom = Mathf.Max(0f, max);
        SetBoredom(Boredom);
    }

    void Awake()
    {
        if (boredomBarRect == null)
        {
            Debug.LogWarning($"{nameof(BoredomBarController)}: boredomBarRect ist nicht gesetzt.");
        }
        else
        {
            // Falls im Inspector nicht gesetzt: aus aktueller Größe übernehmen
            if (Width <= 0f || Height <= 0f)
            {
                var size = boredomBarRect.sizeDelta;
                if (Width <= 0f) Width = Mathf.Max(1f, size.x);
                if (Height <= 0f) Height = Mathf.Max(1f, size.y);
            }
        }
    }

    public void SetBoredom(float boredom)
    {
        Boredom = Mathf.Max(0f, boredom);
        if (boredomBarRect == null) return;
        if (MaxBoredom <= 0f)
        {
            boredomBarRect.sizeDelta = new Vector2(0f, Height);
            return;
        }

        float ratio = Mathf.Clamp01(Boredom / MaxBoredom);
        float newWidth = ratio * Width;
        boredomBarRect.sizeDelta = new Vector2(newWidth, Height);
    }
}
