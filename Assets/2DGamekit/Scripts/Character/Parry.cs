// using UnityEngine;
// using System.Collections;

// public class Parry : MonoBehaviour
// {
//     public bool isInvincible { get; private set; } // public read, private write
//     private bool canParry = true;

//     [Header("Parry Settings")]
//     public float invincibilityTime = 0.2f;
//     public float parryCooldown = 0.6f;
//     public Sprite parrySprite;
//     public SpriteRenderer spriteRenderer;
//     private Coroutine coroutine;

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(1) && canParry)
//         {
//             coroutine = StartCoroutine(Deflect());
//         }
//     }

//     IEnumerator Deflect()
//     {
//         canParry = false;
//         isInvincible = true;
//         // Debug.Log("Parry active! Player is invincible.");
//         spriteRenderer.sprite = parrySprite;
//         yield return new WaitForSeconds(invincibilityTime);

//         isInvincible = false;
//         // Debug.Log("Parry ended. Player vulnerable.");

//         yield return new WaitForSeconds(parryCooldown - invincibilityTime);

//         canParry = true;
//         // Debug.Log("Parry ready again.");
//     }

//     public void ParrySuccess() // gets called via UnityEvents
//     {
//         StopCoroutine(coroutine);
//         canParry = true;
//         isInvincible = false;
//     }
// }









// using UnityEngine;
// using System.Collections;

// public class Parry : MonoBehaviour
// {
//     public bool IsParryActive { get; private set; }     // true only during parry ACTIVE frames
//     public bool IsInvincible  { get; private set; }     // if you also want i-frames tied to parry active
//     public bool CanParry => _state == State.Ready;

//     [Header("Parry Windows (seconds)")]
//     [Tooltip("Startup before the parry becomes active (no i-frames).")]
//     public float startupTime = 0.06f;

//     [Tooltip("Actual parry window. Keep this short.")]
//     public float activeTime = 0.12f;

//     [Tooltip("Recovery if the parry SUCCESSFULLY deflects.")]
//     public float successRecoveryTime = 0.25f;

//     [Tooltip("Recovery if the parry MISSES. Make this longer to punish mashing.")]
//     public float missRecoveryTime = 0.55f;

//     [Header("Visuals")]
//     public Sprite parrySprite;
//     public Sprite defaultSprite;
//     public SpriteRenderer spriteRenderer;

//     private enum State { Ready, Startup, Active, Recovery }
//     private State _state = State.Ready;
//     private bool _parrySucceeded;

//     void Update()
//     {
//         // Only allow starting from Ready. Spamming during startup/active/recovery does nothing.
//         if (Input.GetMouseButtonDown(1) && _state == State.Ready)
//         {
//             StartCoroutine(ParryRoutine());
//         }
//     }

//     /// <summary>Call this from your enemy hit logic if their attack overlaps while IsParryActive is true.</summary>
//     public void OnParrySuccess()
//     {
//         if (_state == State.Active)
//         {
//             _parrySucceeded = true;
//             // Optional: immediately end Active to feel snappier and go to success recovery
//             // StopAllCoroutines(); StartCoroutine(RecoveryRoutine(success:true));
//         }
//     }

//     private IEnumerator ParryRoutine()
//     {
//         _state = State.Startup;
//         _parrySucceeded = false;
//         IsParryActive = false;
//         IsInvincible = false;

//         if (spriteRenderer && parrySprite) spriteRenderer.sprite = parrySprite;

//         // STARTUP: no i-frames yet; bait-and-punish mashing
//         if (startupTime > 0f) yield return new WaitForSeconds(startupTime);

//         // ACTIVE: short, precise timing window
//         _state = State.Active;
//         IsParryActive = true;
//         IsInvincible = true;
//         if (activeTime > 0f) yield return new WaitForSeconds(activeTime);

//         // End active window
//         IsParryActive = false;
//         IsInvincible = false;

//         // RECOVERY: longer if player whiffed, shorter if they actually parried
//         yield return StartCoroutine(RecoveryRoutine(_parrySucceeded));

//         // Back to ready
//         if (spriteRenderer && defaultSprite) spriteRenderer.sprite = defaultSprite;
//         _state = State.Ready;
//     }

//     private IEnumerator RecoveryRoutine(bool success)
//     {
//         _state = State.Recovery;
//         float t = success ? successRecoveryTime : missRecoveryTime;
//         if (t > 0f) yield return new WaitForSeconds(t);
//     }
// }








using UnityEngine;
using System.Collections;

public class Parry : MonoBehaviour
{
    public bool isInvincible { get; private set; }
    private bool canParry = true;

    [Header("Parry Settings")]
    public float invincibilityTime = 0.2f;
    public float parryCooldown = 0.6f;       // total time from press to ready on whiff
    public float successRecovery = 0.25f;    // recommended: short cooldown after a SUCCESS
    public Sprite parrySprite;
    public Sprite defaultSprite;
    public SpriteRenderer spriteRenderer;

    private Coroutine coroutine;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && canParry)
        {
            coroutine = StartCoroutine(Deflect());
        }
    }

    IEnumerator Deflect()
    {
        canParry = false;
        isInvincible = true;
        if (spriteRenderer && parrySprite) spriteRenderer.sprite = parrySprite;

        // ACTIVE (i-frames)
        yield return new WaitForSeconds(invincibilityTime);

        // End active window; now in recovery (whiff case)
        isInvincible = false;
        if (spriteRenderer && defaultSprite) spriteRenderer.sprite = defaultSprite;

        // Miss/whiff recovery = parryCooldown - active time
        float whiffRecovery = Mathf.Max(0f, parryCooldown - invincibilityTime);
        yield return new WaitForSeconds(whiffRecovery);

        canParry = true;
        coroutine = null;
    }

    // Call this from Hitbox when it detects a parry (i.e., during i-frames)
    public void ParrySuccess()
    {
        // stop current routine if running
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        // end i-frames immediately, swap sprite back
        isInvincible = false;
        if (spriteRenderer && defaultSprite) spriteRenderer.sprite = defaultSprite;

        // OPTIONAL: brief success recovery to prevent instant re-parry spam
        StartCoroutine(SuccessRecovery());
    }

    private IEnumerator SuccessRecovery()
    {
        canParry = false;
        if (successRecovery > 0f)
            yield return new WaitForSeconds(successRecovery);
        canParry = true;
    }
}
