using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // This allows any script to call AudioManager.Instance
    public static AudioManager Instance { get; private set; }

    [Header("Audio Components")]
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip successSound;

    private void Awake()
    {
        // Standard Singleton setup
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void PlaySuccess()
    {
        // PlayOneShot allows multiple sounds to overlap naturally
        if (sfxSource != null && successSound != null)
        {
            sfxSource.PlayOneShot(successSound);
        }
    }
}