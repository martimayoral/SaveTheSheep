using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    public AudioClip ambientBirdsSound;

    public AudioClip dogSound;

    public AudioClip grabSound;

    public AudioClip wolfHowlSound;
    public AudioClip wolfAgressiveSound;
    public AudioClip wolfCrySound;
    public AudioClip wolfSmallCrySound;

    public AudioClip sheepSound;

    public AudioClip bellSound;
    public AudioClip gameOverSound;


    private Vector3 cameraPosition;

    private float loopAmbientSoundTime;
    private float ambientLoopTimer;

    private float redDogLoopTimer;
    private float blueDogLoopTimer;

    private float nextRedSound;
    private float nextBlueSound;

    private void Awake()
    {
        Instance = this;
        cameraPosition = Camera.main.transform.position;

        loopAmbientSoundTime = ambientBirdsSound.length;
        ambientLoopTimer = 0.0f;
        PlaySound(ambientBirdsSound);

        redDogLoopTimer = 0.0f;
        blueDogLoopTimer = 0.0f;

        nextRedSound = GetNextSoundTimer();
        nextBlueSound = GetNextSoundTimer();
    }

    public void Update()
    {
        ambientLoopTimer += Time.deltaTime;
        redDogLoopTimer += Time.deltaTime;
        blueDogLoopTimer += Time.deltaTime;

        if (ambientLoopTimer > loopAmbientSoundTime)
        {
            PlaySound(ambientBirdsSound);
            ambientLoopTimer = 0.0f;
        }

        if (redDogLoopTimer > nextRedSound)
        {
            PlaySound(dogSound);
            redDogLoopTimer = 0.0f;
            nextRedSound = GetNextSoundTimer();
        }

        if (blueDogLoopTimer > nextBlueSound)
        {
            PlaySound(dogSound);
            blueDogLoopTimer = 0.0f;

            nextBlueSound = GetNextSoundTimer();
        }

    }

    private float GetNextSoundTimer()
    {
        return Random.Range(10.0f, 20.0f);
    }

    private void PlaySound(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, cameraPosition);
    }

    public void PlayWolfGrabbedClip()
    {
        PlaySound(grabSound);
    }

    public void PlayWolfHowlClip()
    {
        PlaySound(wolfHowlSound);
    }

    public void PlayWolfAgressiveClip()
    {
        PlaySound(wolfAgressiveSound);
    }

    public void PlayWolfCryClip()
    {
        PlaySound(wolfCrySound);
    }


    public void PlayWolfSmallCryClip()
    {
        PlaySound(wolfSmallCrySound);
    }

    public void PlaySheepClip()
    {
        PlaySound(sheepSound);
    }

    public void PlayBellClip()
    {
        PlaySound(bellSound);
    }

    public void PlayGameOverClip()
    {
        PlaySound(gameOverSound);
    }
}
