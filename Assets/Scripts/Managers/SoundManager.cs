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

    [Header("NPC")]
    [SerializeField] private AudioClip _openPanelSound;
    [SerializeField] private AudioClip _buyClickSound;
    [SerializeField] private AudioClip _errorClickSound;
    [Header("Inventory")]
    [SerializeField] private AudioClip _selectedSound;
    [SerializeField] private AudioClip _dropSound;
    [Header("Backpack")]
    [SerializeField] private AudioClip _backpackOpenSound;
    [SerializeField] private AudioClip _backpackCloseSound;
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

    public void PlaySelectedSound() => PlaySFX(_selectedSound);
    public void PlayInventoryDropSound() => PlaySFX(_dropSound);
    public void PlayBackpackOpenSound() => PlaySFX(_backpackOpenSound);
    public void PlayBackpackCloseSound() => PlaySFX(_backpackCloseSound);
    public void PlayOpenPanelSound() => PlaySFX(_openPanelSound);
    public void PlayBuyClick() => PlaySFX(_buyClickSound);
    public void PlayErrorClick() => PlaySFX(_errorClickSound);

}
