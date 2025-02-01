using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SFXSounds
{
    handgun_pickup = 0,
    handgun_shot = 1,
    handgun_reload = 2
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioSource srcPrefab;
    //[SerializeField] AudioSource MusicSource;

    [SerializeField] private int defaultPoolSize = 10;
    private List<AudioSource> inactivePool;
    private List<AudioSource> activePool;

    [Header("--- SFX Sounds ---")]
    [SerializeField] private AudioClip handgunPickup;
    [SerializeField] private AudioClip handgunShot;
    [SerializeField] private AudioClip handgunReload;

    private void Awake()
    {
        activePool = new List<AudioSource>(defaultPoolSize);
        inactivePool = new List<AudioSource>(defaultPoolSize);
        for (int i = 0; i < defaultPoolSize; i++)
        {
            AddSource();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < activePool.Count; i++)
        {
            if (activePool[i] != null && !activePool[i].isPlaying)
            {
                SetActive(activePool[i], false);
            }
        }
    }

    private IEnumerator ClipEnd(float clipLength, AudioSource source)
    {
        yield return new WaitForSeconds(clipLength);

        source.Stop();
        SetActive(source, false);
    }

    public void PlaySFX(SFXSounds sound, Transform origin, bool globalSFX = false)
    {
        AudioSource source = GetVacant();
        SetActive(source, true);

        switch (globalSFX)
        {
            case false:
                source.spatialBlend = 1;
                break;
            case true:
                source.spatialBlend = 0;
                break;
        }

        source.gameObject.transform.parent = origin;
        source.gameObject.transform.localPosition = Vector3.zero;

        switch (sound)
        {
            case SFXSounds.handgun_pickup:
                source.clip = handgunPickup;
                break;
            case SFXSounds.handgun_shot:
                source.clip = handgunShot;
                break;
            case SFXSounds.handgun_reload:
                source.clip = handgunReload;
                break;
        }

        source.Play();
        StartCoroutine(ClipEnd(source.clip.length, source));
    }

    private AudioSource GetVacant()
    {
        for (int i = 0; i < inactivePool.Count; i++)
        {
            if (inactivePool[i] != null)
            {
                return inactivePool[i];
            }
        }
        AddSource();
        return AddSource();
    }

    private void SetActive(AudioSource source, bool active)
    {
        switch (active)
        {
            case true:
                inactivePool.Remove(source);
                activePool.Add(source);
                break;
            case false:
                activePool.Remove(source);
                inactivePool.Add(source);
                source.gameObject.transform.parent = this.transform;
                source.gameObject.transform.localPosition = Vector3.zero;
                break;
        }
        source.gameObject.SetActive(active);
    }

    private AudioSource AddSource()
    {
        AudioSource source = Instantiate(srcPrefab, transform.position, Quaternion.identity, transform);
        inactivePool.Add(source);
        source.gameObject.SetActive(false);

        return source;
    }

    public void AdjustMasterVolume(float level)
    {
        float linearLevel = Mathf.Log10(level) * 20f;
        audioMixer.SetFloat("masterVolume", linearLevel);
    }
    public void AdjustSFXVolume(float level)
    {
        float linearLevel = Mathf.Log10(level) * 20f;
        audioMixer.SetFloat("sfxVolume", linearLevel);
    }
    public void AdjustMusicVolume(float level)
    {
        float linearLevel = Mathf.Log10(level) * 20f;
        audioMixer.SetFloat("musicVolume", linearLevel);
    }
}
