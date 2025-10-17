using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float maxX;
    public float maxY;
    void Start()
    {
        transform.position = player.transform.position;
    }

    void Update()
    {
        // TODO: Interpolate
        Vector3 newPos = player.transform.position;

        // Clamp to world border
        newPos.x = Math.Min(maxX, newPos.x);
        newPos.y = Math.Min(maxY, newPos.y);
        newPos.x = Math.Max(-maxX, newPos.x);
        newPos.y = Math.Max(-maxY, newPos.y);
        newPos.z = -10;

        transform.position = newPos;
    }
}
