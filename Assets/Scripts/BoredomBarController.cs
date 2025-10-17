using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoredomBarController : MonoBehaviour
{
    public float Boredom, MaxBoredom, Width, Height;
    [SerializeField] private RectTransform boredomBarRect;

    public void SetMaxBoredom(float max)
    {
        MaxBoredom = max;
    }

    public void SetBoredom(float boredom)
    {
        Boredom = boredom;
        float newWidth = Boredom / MaxBoredom * Width;
        boredomBarRect.sizeDelta = new Vector2(newWidth, Height);
    }
}
