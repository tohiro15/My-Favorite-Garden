using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Source")]
    [Space]

    [SerializeField] private AudioSource _SFXAudioSource;
    [SerializeField] private AudioSource _MusicAudioSource;

    [Header("Audio Clips")]
    [Space]

    [SerializeField] private AudioClip _buyClickSound;
    [SerializeField] private AudioClip _errorClickSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlaySFX(AudioClip clip)
    {
        _SFXAudioSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        _MusicAudioSource.clip = clip;
        _MusicAudioSource.Play();
    }

    public void StopMusic()
    {
        _MusicAudioSource.Stop();
    }

    public void PlayBuyClick() => PlaySFX(_buyClickSound);
    public void PlayErrorClick() => PlaySFX(_errorClickSound);

}
