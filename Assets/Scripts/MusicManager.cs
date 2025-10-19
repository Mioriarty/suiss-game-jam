using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip boringBg;
    public AudioClip excitingBg;

    private bool isExciting = false;

    private AudioSource audioSource;

    private static MusicManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = boringBg;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SetExciting(bool exciting)
    {
        if (isExciting == exciting)
            return;

        isExciting = exciting;

        if (isExciting)
        {
            audioSource.clip = excitingBg;
        }
        else
        {
            audioSource.clip = boringBg;
        }

        audioSource.Play();
    }
}
