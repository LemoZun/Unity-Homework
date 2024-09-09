using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource switchSound;
    [SerializeField] AudioSource gunfireSound;
    
    //private int sfxIndex = 0;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayBGM(AudioClip _clip)
    {
        bgm.clip = _clip;
        bgm.Play();
    }

    public void PlaySwitchSound(AudioClip _clip)
    {
        switchSound.PlayOneShot(_clip);
    }

    public void PlayGunFireSound(AudioClip _clip)
    {
        gunfireSound.PlayOneShot(_clip);
    }

}
