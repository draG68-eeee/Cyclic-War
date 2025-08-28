// using UnityEngine;
// using System.Collections;

// public class UpgradedEllaFinalFightAI : MonoBehaviour
// {
//     public GameObject Alle;
//     public Hitbox DashHitbox;
//     public AlleBossAI alleBossAI;
//     // public Damageable alleDamageable;
//     private SpriteRenderer spriteRenderer;
//     public Sprite idle, guard, stagger;
//     public Sprite[] attack1;

//     public Rigidbody2D rb;
//     public NPCAllyHitbox attackHitboxR, attackHitboxL;

//     private bool canStartCombo;
//     private Coroutine attackCoroutine;


//     void Start()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         StartCoroutine(WaitForDoubleThrust());
//         StartCoroutine(AttackSystem());
//     }

//     IEnumerator WaitForDoubleThrust()
//     {
//         while (true)
//         {
//             if (alleBossAI.currentAttack == 3 && alleBossAI.victim == this.gameObject)
//             {
//                 if (attackCoroutine != null)
//                 {
//                     StopCoroutine(attackCoroutine);
//                 }
//                 yield return CounterDoubleThrust();

//             }
//             yield return new WaitForSeconds(0.1f);
//         }
//     }

//     IEnumerator AttackSystem()
//     {
//         while (true)
//         {
//             if (alleBossAI.victim != this.gameObject)
//             {
//                 Debug.Log("attacking");
//                 attackCoroutine = StartCoroutine(AttackAlle());
//                 yield return attackCoroutine;
//                 Debug.Log("finished attacking");
//                 attackCoroutine = null;
//             }
//             yield return new WaitForSeconds(0.1f);
//         }
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.gameObject != Alle && !other.gameObject.CompareTag("Projectile"))
//         {
//             return;
//         }

//         if (attackCoroutine != null)
//         {
//             StopCoroutine(attackCoroutine);
//         }

//         if (alleBossAI.currentAttack == 1 && DashHitbox.isHitboxActive == true)
//         {


//             spriteRenderer.sprite = guard;
//             StartCoroutine(AttackAlle());

//         }

//         if (alleBossAI.currentAttack == 2 && other.GetComponent("Hitbox"))
//         {
//             spriteRenderer.sprite = guard;
//             StartCoroutine(AttackAlle());
//         }

//         // if (alleBossAI.currentAttack == 3 && canStartCombo)
//         // {

//         //     StartCoroutine(CounterDoubleThrust());
//         // }



//     }

//     // IEnumerator CounterDoubleThrust()
//     // {
//     //     canStartCombo = false;

//     // }

//     IEnumerator AttackAlle()
//     {
//         yield return new WaitForSeconds(0.3f);
//         Vector3 dir = (Alle.transform.position - transform.position).normalized;
//         spriteRenderer.flipX = dir.x < 0;

//         spriteRenderer.sprite = attack1[0];
//         yield return new WaitForSeconds(0.5f);

//         spriteRenderer.sprite = attack1[1];

//         // move + enable the correct hitbox
//         rb.linearVelocity = new Vector2(dir.x * 100f, 0f); // <-- fixed
//         if (spriteRenderer.flipX)
//         {
//             attackHitboxL.ResetHit();
//             attackHitboxL.isHitboxActive = true;
//         }
//         else
//         {
//             attackHitboxR.ResetHit();
//             attackHitboxR.isHitboxActive = true;
//         }

//         yield return new WaitForSeconds(0.2f);

//         // cleanup
//         rb.linearVelocity = Vector2.zero; // <-- fixed
//         attackHitboxL.isHitboxActive = false;
//         attackHitboxR.isHitboxActive = false;
//         spriteRenderer.sprite = idle;
//         yield break;
//     }

//     IEnumerator CounterDoubleThrust()
//     {
//         // Debug.Log("this works");
//         Vector3 dir = (Alle.transform.position - transform.position).normalized;
//         rb.linearVelocity = new Vector2(dir.x * -20, 0);
//         yield return new WaitForSeconds(0.3f);
//         rb.linearVelocity = Vector2.zero;
//         yield return AttackAlle();
//         yield return new WaitForSeconds(2);

//     }
// }



    

using UnityEngine;
using System.Collections;

public class UpgradedEllaFinalFightAI : MonoBehaviour
{

    [Header("Lunge Settings")]
    public float lungeWindup = 0.5f;      // time showing attack1[0]
    public float lungeTime = 0.18f;     // travel duration
    public float overshoot = 0.35f;     // how far past contact to try to go
    public bool allowPassThrough = false; // set true to slip behind Alle during lunge
    public LayerMask lineOfAttackMask;    // include Alle + walls/ground

    [Header("Refs")]
    public GameObject Alle;
    public AlleBossAI alleBossAI;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Sprites")]
    public Sprite idle, guard, stagger;
    public Sprite[] attack1;

    [Header("Hitboxes")]
    public Hitbox DashHitbox;                 // Alle’s dash hitbox reference
    public NPCAllyHitbox attackHitboxR, attackHitboxL;

    // control
    private Coroutine attackCoroutine;
    private bool actionLocked;                // prevents re-entrancy (global cooldown)
    private readonly WaitForSeconds poll = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds tinyDelay = new WaitForSeconds(0.3f);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(WaitForDoubleThrust());
        StartCoroutine(AttackSystem());
    }

    IEnumerator WaitForDoubleThrust()
    {
        while (true)
        {
            // Counter Alle's "DoubleThrust" (currentAttack == 3) ONLY if target is Ella
            if (alleBossAI != null
                && alleBossAI.currentAttack == 3
                && alleBossAI.victim == this.gameObject
                && !actionLocked)
            {
                // interrupt current attack safely
                InterruptCurrentAttack();

                yield return StartCoroutine(CounterDoubleThrust());
            }

            yield return poll;
        }
    }

    IEnumerator AttackSystem()
    {
        while (true)
        {
            // If Alle is focused elsewhere, we can attack
            if (alleBossAI != null && alleBossAI.victim != this.gameObject && !actionLocked && attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackAlle());
                yield return attackCoroutine;   // wait until it finishes
                attackCoroutine = null;
            }
            yield return poll;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to Alle or Alle projectiles
        if (other.gameObject != Alle && !other.gameObject.CompareTag("Projectile"))
            return;

        if (actionLocked) return; // don’t spam reactions

        // Defensive reactions to certain Alle attacks
        // 1) Dash (currentAttack == 1) AND Alle’s dash hitbox currently active
        if (alleBossAI != null && alleBossAI.currentAttack == 1 && DashHitbox != null && DashHitbox.isHitboxActive)
        {
            InterruptCurrentAttack();
            StartCoroutine(GuardThenPunish());
            return;
        }

        // 2) Generic melee (currentAttack == 2) if this collider is a Hitbox
        if (alleBossAI != null && alleBossAI.currentAttack == 2 && other.GetComponent<Hitbox>() != null)
        {
            InterruptCurrentAttack();
            StartCoroutine(GuardThenPunish());
            return;
        }
    }

    // --- Moves ---

    IEnumerator GuardThenPunish()
    {
        actionLocked = true;

        // brief guard pose
        spriteRenderer.sprite = guard;
        yield return tinyDelay;

        // immediate punish attack
        yield return StartCoroutine(AttackAlle());

        // short global cooldown to avoid thrashing
        yield return new WaitForSeconds(0.25f);
        actionLocked = false;
    }

    IEnumerator AttackAlle()
    {
        actionLocked = true;

        // tiny tell
        yield return tinyDelay;
        if (Alle == null) { actionLocked = false; yield break; }

        // face target
        Vector2 start = rb.position;
        Vector2 toAlle = ((Vector2)Alle.transform.position - start).normalized;
        spriteRenderer.flipX = toAlle.x < 0f;

        // windup
        if (attack1 != null && attack1.Length > 0) spriteRenderer.sprite = attack1[0];
        yield return new WaitForSeconds(lungeWindup);

        // strike pose
        if (attack1 != null && attack1.Length > 1) spriteRenderer.sprite = attack1[1];

        // compute destination: raycast to first obstacle/Alle; try to go a bit beyond
        Vector2 dest = ComputeLungeDestination(start, toAlle);

        // enable correct hitbox just before movement
        if (spriteRenderer.flipX)
        {
            attackHitboxL.ResetHit();
            attackHitboxL.isHitboxActive = true;
        }
        else
        {
            attackHitboxR.ResetHit();
            attackHitboxR.isHitboxActive = true;
        }

        // optionally ignore collision with Alle to pass through
        Collider2D myCol = GetComponent<Collider2D>();
        Collider2D alleCol = Alle ? Alle.GetComponent<Collider2D>() : null;
        if (allowPassThrough && myCol && alleCol) Physics2D.IgnoreCollision(myCol, alleCol, true);

        // smooth dash with MovePosition
        float t = 0f;
        while (t < lungeTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / lungeTime);
            rb.MovePosition(Vector2.Lerp(start, dest, a));
            yield return null;
        }

        // re-enable collisions if we ignored them
        if (allowPassThrough && myCol && alleCol) Physics2D.IgnoreCollision(myCol, alleCol, false);

        // short linger with hitbox still active to catch late contacts
        yield return new WaitForSeconds(0.06f);

        // cleanup
        attackHitboxL.isHitboxActive = false;
        attackHitboxR.isHitboxActive = false;
        rb.linearVelocity = Vector2.zero;
        spriteRenderer.sprite = idle;

        // recovery
        yield return new WaitForSeconds(0.15f);
        actionLocked = false;
    }

    IEnumerator CounterDoubleThrust()
    {
        actionLocked = true;

        // step back to bait/evade
        if (Alle != null)
        {
            Vector3 dir = (Alle.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(-dir.x * 20f, 0f);
        }

        yield return new WaitForSeconds(0.3f);
        rb.linearVelocity = Vector2.zero;

        // punish
        yield return StartCoroutine(AttackAlle());

        // longer recovery after a counter so it doesn’t chain forever
        yield return new WaitForSeconds(2f);
        actionLocked = false;
    }

    // --- Utilities ---

    void InterruptCurrentAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        // safety cleanup
        rb.linearVelocity = Vector2.zero;
        if (attackHitboxL != null) attackHitboxL.isHitboxActive = false;
        if (attackHitboxR != null) attackHitboxR.isHitboxActive = false;
        spriteRenderer.sprite = guard; // show guard if we’re interrupting due to a defend
    }
    
    Vector2 ComputeLungeDestination(Vector2 start, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(start, dir, 100f, lineOfAttackMask);

        Vector2 final;
        if (hit.collider != null)
        {
            bool hitIsAlle = hit.collider.gameObject == Alle;
            float push = hitIsAlle ? overshoot : 0f;
            final = hit.point + dir * push;
        }
        else
        {
            final = (Vector2)Alle.transform.position + dir * overshoot;
        }

        // --- lock Y to Ella's current Y so she only moves horizontally ---
        final.y = start.y;
        return final;
    }


}
