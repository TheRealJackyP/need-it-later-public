using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public static SFXHandler Instance;
    public AudioSource TargetSource;
    public AudioSource ShootSource;
    public SerializableDictionary<string, AudioClip> SFXMapping;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        
          
        Instance = this;
    }

    public void PlaySFX(string clipName)
    {
        if(SFXMapping.TryGetValue(clipName, out var clip))
            TargetSource.PlayOneShot(clip);
    }

    // public void PlaySFXRandomPitch(string clipName)
    // {
    //     var newSource = gameObject.AddComponent<AudioSource>();
    //     // var pitch = Random.Range(.1f, 3f) * (Random.value > .5 ? 1 : -1);
    //     var pitch = Random.Range(.9f, 1.1f);
    //     var clip = SFXMapping[clipName];
    //     newSource.pitch = pitch;
    //     newSource.clip = clip;
    //     newSource.Play();
    //     Destroy(newSource, clip.length / Mathf.Abs(pitch));
    // }

    public void PlayShootSFX()
    {
        // PlaySFXRandomPitch("Shoot");
        // ShootSource.pitch = Random.Range(-1, 1);
        ShootSource.PlayOneShot(SFXMapping["Shoot"]);
    }

    public void PlayHitSFX()
    {
        ShootSource.PlayOneShot(SFXMapping["ProjectileHit"]);
    }
}
