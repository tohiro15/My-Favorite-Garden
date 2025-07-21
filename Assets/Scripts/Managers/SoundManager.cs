using System.Collections;
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

    [Header("Music")]
    [Space]

    [SerializeField] private AudioClip[] _musicClips;
    private int _currentMusic = 0;
    [Header("SFX")]
    [Space]

    [Header("NPC")]
    [SerializeField] private AudioClip _openPanelSound;
    [SerializeField] private AudioClip _buyClickSound;
    [SerializeField] private AudioClip _errorClickSound;
    [Header("Plant")]
    [SerializeField] private AudioClip[] _digClips;
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
    private void Start()
    {
        StartCoroutine(PlayMusicSequence());
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
    private IEnumerator PlayMusicSequence()
    {
        while (true)
        {
            if (_musicClips.Length == 0) yield break;

            _MusicAudioSource.clip = _musicClips[_currentMusic];
            _MusicAudioSource.Play();

            yield return new WaitForSeconds(_musicClips[_currentMusic].length);

            _currentMusic++;
            if (_currentMusic >= _musicClips.Length)
                _currentMusic = 0;
        }
    }

    public void PlayDigSound()
    {
        int randomClip = Random.Range(0, _digClips.Length);
        PlaySFX(_digClips[randomClip]);
    }
    public void PlaySelectedSound() => PlaySFX(_selectedSound);
    public void PlayInventoryDropSound() => PlaySFX(_dropSound);
    public void PlayBackpackOpenSound() => PlaySFX(_backpackOpenSound);
    public void PlayBackpackCloseSound() => PlaySFX(_backpackCloseSound);
    public void PlayOpenPanelSound() => PlaySFX(_openPanelSound);
    public void PlayBuyClick() => PlaySFX(_buyClickSound);
    public void PlayErrorClick() => PlaySFX(_errorClickSound);

}
