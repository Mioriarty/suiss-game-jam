using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float maxX;
    public float maxY;
    private Vector3 smoothPos;
    private Vector3 newPos;
    private float smoothSpeed = .05f;

    void Start()
    {
        transform.position = player.transform.position;
    }

    void Update()
    {
        newPos = player.transform.position;

        // Clamp to world border
        newPos.x = Math.Min(maxX, newPos.x);
        newPos.y = Math.Min(maxY, newPos.y);
        newPos.x = Math.Max(-maxX, newPos.x);
        newPos.y = Math.Max(-maxY, newPos.y);
        newPos.z = -10;

        // transform.position = newPos;
    }

    void LateUpdate()
    {
        // Interpolate
        smoothPos = Vector3.Lerp(transform.position, newPos, smoothSpeed);
        transform.position = smoothPos;
    }
}
