using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static PlayerSound instance;
    AudioSource audioSource;

    public AudioClip jump;
    public AudioClip hurt;
    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
