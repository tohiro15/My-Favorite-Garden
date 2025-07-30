using UnityEngine;
using UnityEngine.Audio;

public class MainMenuSoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _sfxMixer;
    [SerializeField] private AudioMixer _musicMixer;

    [Range(0f, 1f)] private float _sfxVolume = 1.0f;
    [Range(0f, 1f)] private float _musicVolume = 1.0f;
    public AudioMixer GetSFXMixer => _sfxMixer;
    public AudioMixer GetMusicMixer => _musicMixer;
    public float GetSFXVolume => _sfxVolume;
    public float GetMusicVolume => _musicVolume;

    public void LoadVolume()
    {
        ChangeSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
        ChangeMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.8f));
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
        PlayerPrefs.Save();
    }

    public void ChangeSFXVolume(float volume)
    {
        _sfxVolume = volume;
        float dB = Mathf.Lerp(-80f, 0f, volume);
        _sfxMixer.SetFloat("SFXVolume", dB);
    }

    public void ChangeMusicVolume(float volume)
    {
        _musicVolume = volume;
        float dB = Mathf.Lerp(-80f, 0f, volume);
        _musicMixer.SetFloat("MusicVolume", dB);
    }
}
