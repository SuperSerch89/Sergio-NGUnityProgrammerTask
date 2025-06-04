using UnityEngine;
using NicoUtilities;
using UnityEngine.Audio;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixer mainMixer = null;
    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private AudioSource sfxSource = null;
    [SerializeField] private string musicMixerName = "MusicVolume";
    [SerializeField] private string sfxMixerName = "SFXVolume";

    public void PlayMusic(AudioClip clip) => StartCoroutine(SwitchMusicCoroutine(clip));
    public void PlaySFX(AudioClip clip) => sfxSource.PlayOneShot(clip);
    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat(musicMixerName, Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f);
    }
    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat(sfxMixerName, Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f);
    }

    private IEnumerator SwitchMusicCoroutine(AudioClip newClip)
    {
        yield return StartCoroutine(FadeMixer(mainMixer, musicMixerName, -80f, 1f));
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();
        yield return StartCoroutine(FadeMixer(mainMixer, musicMixerName, 0f, 1f));
    }
    public IEnumerator FadeMixer(AudioMixer mixer, string exposedParam, float targetVolumeDb, float duration)
    {
        mixer.GetFloat(exposedParam, out float currentVolume);
        float time = 0f;

        while (time < duration)
        {
            float newVolume = Mathf.Lerp(currentVolume, targetVolumeDb, time / duration);
            mixer.SetFloat(exposedParam, newVolume);
            time += Time.deltaTime;
            yield return null;
        }

        mixer.SetFloat(exposedParam, targetVolumeDb);
    }
}
