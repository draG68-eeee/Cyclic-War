using UnityEngine;
using System.Collections;
using Gamekit2D;
public class MusicArranger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GolemKnightMusicPlayer golemKnightMusicPlayer;
    public Damageable damageable;
    public GolemKnightAI golemKnightAI;
    private Coroutine coroutine;

    void Start()
    {
        // StartCoroutine(Play());
        damageable = GetComponent<Damageable>();
        StartCoroutine(TransitionPhases());
        // StartCoroutine(Death());/

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator TransitionPhases()
    {
        yield return new WaitUntil(() => damageable.CurrentHealth <= 25);
        golemKnightMusicPlayer.StopMusic();
        StopCoroutine(coroutine);
        Begin();
    }
    public void Begin()
    {
        Debug.Log("MusicArranger: Begin called");
        // Prevent multiple Play coroutines
        if (coroutine != null)
        {
            Debug.LogWarning("MusicArranger: Play coroutine already running, stopping previous one.");
            StopCoroutine(coroutine);
            golemKnightMusicPlayer.StopMusic();
            coroutine = null;
        }
        coroutine = StartCoroutine(Play());
    }
    public void End()
    {
        Debug.Log("MusicArranger: End called");
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            golemKnightMusicPlayer.StopMusic();
        }
    }
    public IEnumerator Play()
    {
        if (golemKnightMusicPlayer == null)
        {
            Debug.LogError("golemKnightMusicPlayer is not assigned!");
            yield break;
        }
        if (damageable == null)
        {
            Debug.LogError("damageable is not assigned!");
            yield break;
        }

        // Play intro (phase 1 = intro)
        if (damageable.CurrentHealth == 50)
        {
            yield return StartCoroutine(golemKnightMusicPlayer.PlayMusic(1));
        }
        // yield return new WaitForSeconds(5f); // Intro: 5 seconds

            // Phase 1 (health > 10)
            while (damageable.CurrentHealth > 25)
            {
                yield return StartCoroutine(golemKnightMusicPlayer.PlayMusic(2)); // Phase 1 music
                                                                                  // yield return new WaitForSeconds(17f); // Phase 1 duration
            }

        // Final phase (health <= 10)
        while (damageable.CurrentHealth <= 25)
        {
            yield return StartCoroutine(golemKnightMusicPlayer.PlayMusic(3)); // Final phase music
            // yield return new WaitForSeconds(24f); // Final phase duration
        }
    }
    

}
