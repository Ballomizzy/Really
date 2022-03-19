using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField]
    private AudioClip mainMusic, unrealWorldMusic;

    [Header("SFX")]
    [SerializeField]
    private List<SoundEffect> soundEffects = new List<SoundEffect>();

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip sfxClip;
    }

    [Space]
    [Header("Other References")]
    [SerializeField]
    private GameSettings gameSettings;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainMusic;
        audioSource.Play();
        audioSource.loop = true;
        audioSource.volume = gameSettings.GetGameVolumeValue();
    }

    public void ChangeVolume() 
    {
        audioSource.volume = gameSettings.GetGameVolumeValue();
    }

    public void SwitchMusic(bool unreal)
    {
        if(unreal)
            audioSource.clip = unrealWorldMusic;
        else
            audioSource.clip = mainMusic;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void PlaySFX(string name, Vector3 point)
    {
        for(int i = 0; i < soundEffects.Count; i++)
        {
            if(soundEffects[i].name == name)
            {
                audioSource.PlayOneShot(soundEffects[i].sfxClip, 1.0f);
                Debug.Log("I played " + soundEffects[i].sfxClip.name);
            }
        }
    }

    public void PlaySFX(string name)
    {
        for (int i = 0; i < soundEffects.Count; i++)
        {
            if (soundEffects[i].name == name)
            {
                audioSource.PlayOneShot(soundEffects[i].sfxClip, 1.0f);
                Debug.Log("I played " + soundEffects[i].sfxClip.name);
            }
        }
    }
}
