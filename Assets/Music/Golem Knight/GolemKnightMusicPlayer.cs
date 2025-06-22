using UnityEngine;
using System.Collections;

public class GolemKnightMusicPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioClip intro;
    public AudioClip firstPhase;
    public AudioClip secondPhase;
    public AudioSource audioSource;
    // private int phase;
    // public int currentPhase = 0; // add this to track currently playing phase

    void Start()
    {
        
    }

    // Update is called once per frame
    public IEnumerator PlayMusic(int phase)
    {
        switch (phase)
        {
            case 1:
                audioSource.clip = intro;
                audioSource.Play();
                yield return new WaitForSeconds(6);
                audioSource.Stop();
                break;

            case 2:
                audioSource.Stop();
                audioSource.clip = firstPhase;
                audioSource.Play();
                yield return new WaitForSeconds(24);
                audioSource.Stop();
                break;
            case 3:
                audioSource.Stop();
                audioSource.clip = secondPhase;
                audioSource.Play();
                yield return new WaitForSeconds(17);
                audioSource.Stop();
                break;
            default:
                break;

        }
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }

    void Update()
    {
        
    }
}
