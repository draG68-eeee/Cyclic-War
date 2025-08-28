using UnityEngine;
using System.Collections;

public class EllaFinalFightAI : MonoBehaviour
{
    public GameObject Alle;
    public AlleBossAI alleBossAI;
    // public Damageable alleDamageable;
    private SpriteRenderer spriteRenderer;
    public Sprite idle, guard, stagger;
    public Sprite[] attack1;

    public Rigidbody2D rb;
    public NPCAllyHitbox attackHitboxR, attackHitboxL;

    // Coroutine/state handles
    private Coroutine currentAttack;
    private Coroutine waitUntilSpearThrowDone;
    private Coroutine spearCounterContinue;
    private bool isReacting;
    private bool hasActiveSpearCounter;

    // NEW: explicit completion flag for spear sequence
    private bool spearResolved;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // --- Spear reaction coroutines ---

    IEnumerator CounterSpearThrow()
    {
        // Waiting window during which a projectile hit can trigger the counter
        spriteRenderer.sprite = idle;
        yield return new WaitForSeconds(5f);

        // Natural end (no block occurred)
        waitUntilSpearThrowDone = null;
        hasActiveSpearCounter = false;

        // Signal completion to the parent waiter
        spearResolved = true;
    }

    IEnumerator SpearCounterContinue()
    {
        // tiny delay to “catch” the guard pose
        yield return new WaitForSeconds(0.3f);

        // launch counter-attack
        currentAttack = StartCoroutine(AttackAlle());
        yield return currentAttack;
        currentAttack = null;
        Debug.Log("counterlaunched");

        // If the spear wait is still running, stop it
        if (waitUntilSpearThrowDone != null)
        {
            Debug.Log("finished2");
            StopCoroutine(waitUntilSpearThrowDone);
            waitUntilSpearThrowDone = null;
        }

        // Signal completion to the parent waiter
        spearResolved = true;

        // Clear flags and handle
        spearCounterContinue = null;
        hasActiveSpearCounter = false;
        isReacting = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to Alle's spear projectiles
        if (other.TryGetComponent<AlleSpearProjectile>(out var spear))
        {
            // Only counter if we are in an active spear window and not already countering
            if (hasActiveSpearCounter && spearCounterContinue == null)
            {
                spriteRenderer.sprite = guard;
                spearCounterContinue = StartCoroutine(SpearCounterContinue());
            }
        }
    }

    public void CallEllaAI()
    {
        StartCoroutine(EllaAI());
    }

    // Safely cancel any running attack and reset transient state
    private void CancelCurrentAttack()
    {
        if (currentAttack != null)
        {
            StopCoroutine(currentAttack);
            currentAttack = null;
        }
        rb.linearVelocity = Vector2.zero; // <-- fixed
        attackHitboxL.isHitboxActive = false;
        attackHitboxR.isHitboxActive = false;
        spriteRenderer.sprite = idle;
    }

    public IEnumerator EllaAI()
    {
        while (true)
        {
            // Face Alle
            Vector3 direction = (Alle.transform.position - transform.position).normalized;
            spriteRenderer.flipX = direction.x < 0;

            // --- Safety: if we were reacting to spear but boss changed state, bail out
            if (isReacting && alleBossAI.currentAttack != 2)
            {
                if (waitUntilSpearThrowDone != null)
                {
                    StopCoroutine(waitUntilSpearThrowDone);
                    waitUntilSpearThrowDone = null;
                }
                if (spearCounterContinue != null)
                {
                    StopCoroutine(spearCounterContinue);
                    spearCounterContinue = null;
                }
                hasActiveSpearCounter = false;
                spearResolved = true; // ensure any waiter unwinds
                isReacting = false;
            }

            // React to dash thrust (id 1)
            if (!isReacting && alleBossAI.currentAttack == 1 && alleBossAI.victim == this.gameObject)
            {
                isReacting = true;
                CancelCurrentAttack();
                yield return CounterDashThrust();
                isReacting = false;
            }
            // React to spear throw (id 2)
            else if (!isReacting && alleBossAI.currentAttack == 2 && alleBossAI.victim == this.gameObject)
            {
                isReacting = true;
                CancelCurrentAttack();

                // OPEN spear counter window immediately so OnTriggerEnter can catch the next hit
                hasActiveSpearCounter = true;

                // Reset resolution flag, start wait if needed
                spearResolved = false;
                if (waitUntilSpearThrowDone == null)
                    waitUntilSpearThrowDone = StartCoroutine(CounterSpearThrow());

                // Wait until spear sequence resolves one way or another
                yield return new WaitUntil(() => spearResolved);

                // Close the window and reset
                waitUntilSpearThrowDone = null;
                hasActiveSpearCounter = false;
                isReacting = false;
                Debug.Log("finished");
            }
            // Otherwise, attack if free
            else if (alleBossAI.victim != this.gameObject && currentAttack == null && !isReacting)
            {
                currentAttack = StartCoroutine(AttackAlle());
                yield return currentAttack;
                currentAttack = null;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // --- Other reactions/attacks ---

    IEnumerator CounterDashThrust()
    {
        Debug.Log("countering thrust");
        yield return new WaitForSeconds(2f);
        spriteRenderer.sprite = guard;
        yield return new WaitForSeconds(1f);
        spriteRenderer.sprite = idle;

        currentAttack = StartCoroutine(AttackAlle());
        yield return currentAttack;
        currentAttack = null;

        yield return new WaitForSeconds(2.1f);
    }

    IEnumerator AttackAlle()
    {
        Vector3 dir = (Alle.transform.position - transform.position).normalized;
        spriteRenderer.flipX = dir.x < 0;

        spriteRenderer.sprite = attack1[0];
        yield return new WaitForSeconds(0.5f);

        spriteRenderer.sprite = attack1[1];

        // move + enable the correct hitbox
        rb.linearVelocity = new Vector2(dir.x * 100f, 0f); // <-- fixed
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

        yield return new WaitForSeconds(0.2f);

        // cleanup
        rb.linearVelocity = Vector2.zero; // <-- fixed
        attackHitboxL.isHitboxActive = false;
        attackHitboxR.isHitboxActive = false;
        spriteRenderer.sprite = idle;
    }
}
