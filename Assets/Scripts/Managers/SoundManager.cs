using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    public AudioClip wolfSound;
    public AudioClip sheepSound;

    private Vector3 cameraPosition;

    private void Awake()
    {
        Instance = this;
        cameraPosition = Camera.main.transform.position;
    }

    private void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, cameraPosition);
    }

    public void PlayWolfClip()
    {
        PlaySound(wolfSound);
    }

    public void PlaySheepClip()
    {
        PlaySound(sheepSound);
    }
}
