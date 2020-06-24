using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IManager
{
    #region Properties
    [SerializeField] protected AudioSource bgAudioSource;
    [SerializeField] protected AudioSource sfxAudioSource;
    [SerializeField] protected AudioClip bgClip;
    [SerializeField] protected AudioClip selectClip;
    [SerializeField] protected AudioClip clearClip;
    [SerializeField] protected AudioClip swapClip;
    #endregion

    #region IManager
    public void Init()
    {
        bgAudioSource.loop = true;
        bgAudioSource.clip = bgClip;
        bgAudioSource.Play();
    }
    #endregion

    #region Public
    public void Select()
    {
        sfxAudioSource.PlayOneShot(selectClip);
    }

    public void Clear()
    {
        sfxAudioSource.PlayOneShot(clearClip);
    }

    public void Swap()
    {
        sfxAudioSource.PlayOneShot(swapClip);
    }

    public void PauseSound()
    {
        bgAudioSource.Pause();
    }

    public void ResumeSound()
    {
        bgAudioSource.UnPause();
    }
    #endregion
}
