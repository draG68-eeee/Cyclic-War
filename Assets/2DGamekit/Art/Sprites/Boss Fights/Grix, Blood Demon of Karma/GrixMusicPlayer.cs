using UnityEngine;

public class GrixMusicPlayer : MonoBehaviour
{
    [Header("Music Clips")]
    public AudioClip introMusic;
    public AudioClip phase1Music;
    public AudioClip phase2Music;
    public AudioSource audioSource;

    [Header("Loop Points (seconds)")]
    public float phase1LoopStart = 0f;
    public float phase1LoopEnd = 40f;
    public float phase2LoopStart = 0f;
    public float phase2LoopEnd = 56f;

    private enum MusicPhase { None, Intro, Phase1, Phase2 }
    private MusicPhase currentPhase = MusicPhase.None;

    private bool introPlayed = false;
    private bool phase1Played = false;
    private bool phase2Played = false;

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
        }
    }

    void Update()
    {
        // Only custom loop for phase 1 and 2
        if (currentPhase == MusicPhase.Phase1 && audioSource.isPlaying)
        {
            if (audioSource.time >= phase1LoopEnd)
            {
                audioSource.time = phase1LoopStart;
                audioSource.Play();
            }
        }
        else if (currentPhase == MusicPhase.Phase2 && audioSource.isPlaying)
        {
            if (audioSource.time >= phase2LoopEnd)
            {
                audioSource.time = phase2LoopStart;
                audioSource.Play();
            }
        }
    }

    public void PlayIntro()
    {
        if (introMusic != null && !introPlayed)
        {
            audioSource.clip = introMusic;
            audioSource.loop = false;
            audioSource.time = 0f;
            audioSource.Play();
            introPlayed = true;
            phase1Played = false;
            phase2Played = false;
            currentPhase = MusicPhase.Intro;
        }
    }

    public void PlayPhase1()
    {
        if (phase1Music != null && !phase1Played)
        {
            audioSource.clip = phase1Music;
            audioSource.loop = false; // custom loop
            audioSource.time = phase1LoopStart;
            audioSource.Play();
            phase1Played = true;
            phase2Played = false;
            currentPhase = MusicPhase.Phase1;
        }
    }

    public void PlayPhase2()
    {
        if (phase2Music != null && !phase2Played)
        {
            audioSource.clip = phase2Music;
            audioSource.loop = false; // custom loop
            audioSource.time = phase2LoopStart;
            audioSource.Play();
            phase2Played = true;
            currentPhase = MusicPhase.Phase2;
        }
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}

