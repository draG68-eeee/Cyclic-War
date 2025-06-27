using UnityEngine;
using System.Collections;

public class GolemKnightMusicPlayer : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip firstPhase;
    public AudioClip secondPhase;
    public AudioSource audioSource;

    private Coroutine loopCoroutine;
    private int currentLoopPhase = 0;
    private bool stopLoopRequested = false;

    // These should match the actual musical content length (not the file length)
    public float firstPhaseMusicDuration = 24f; // e.g. 24 seconds
    public float secondPhaseMusicDuration = 17f; // e.g. 17 seconds

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // public AudioClip intro;
    // public AudioClip firstPhase;
    // public AudioClip secondPhase;
    // public AudioSource audioSource;
    // private int phase;
    // public int currentPhase = 0; // add this to track currently playing phase

    void Start()
    {
        
    }

    // Update is called once per frame
    public IEnumerator PlayMusic(int phase)
    {
        // Stop any previous manual loop
        StopManualLoop();
        switch (phase)
        {
            case 1:
                audioSource.loop = false;
                audioSource.clip = intro;
                audioSource.Play();
                yield return new WaitForSeconds(6);
                audioSource.Stop();
                break;
            case 2:
                StartManualLoop(2);
                // Wait until loop is stopped externally (by phase change or StopMusic)
                while (currentLoopPhase == 2 && !stopLoopRequested)
                {
                    yield return null;
                }
                break;
            case 3:
                StartManualLoop(3);
                while (currentLoopPhase == 3 && !stopLoopRequested)
                {
                    yield return null;
                }
                break;
            default:
                break;
        }
    }

    private void StartManualLoop(int phase)
    {
        StopManualLoop();
        stopLoopRequested = false;
        currentLoopPhase = phase;
        loopCoroutine = StartCoroutine(ManualLoopCoroutine(phase));
    }

    private void StopManualLoop()
    {
        stopLoopRequested = true;
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
        currentLoopPhase = 0;
    }

    private IEnumerator ManualLoopCoroutine(int phase)
    {
        AudioClip clip = null;
        float musicDuration = 0f;
        switch (phase)
        {
            case 2:
                clip = firstPhase;
                musicDuration = firstPhaseMusicDuration;
                break;
            case 3:
                clip = secondPhase;
                musicDuration = secondPhaseMusicDuration;
                break;
            default:
                yield break;
        }
        audioSource.loop = false;
        audioSource.clip = clip;
        while (!stopLoopRequested && currentLoopPhase == phase)
        {
            audioSource.Play();
            yield return new WaitForSeconds(musicDuration);
            audioSource.Stop();
            // Optionally add a very short delay to avoid audio glitches
            yield return null;
        }
    }
    public void StopMusic()
    {
        StopManualLoop();
        audioSource.Stop();
    }

    void Update()
    {
        
    }
}
