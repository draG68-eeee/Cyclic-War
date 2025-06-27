using UnityEngine;
using System.Collections;
using Gamekit2D;
public class PaleDragonAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public Sprite[] clawSweepSprites;
    public Sprite[] diveBombSprites;
    public Sprite[] tailPierceSprites;
    public Sprite idle;
    public Damageable damageable;


    public Hitbox tailJabHitboxL;
    public Hitbox tailJabHitboxR;
    public Hitbox clawSweepHitboxL;
    public Hitbox clawSweepHitboxR;
    public Hitbox divebombHitbox;
    public UpdateHPBar updateHPBar;

    public bool isAggro;


    // TRUE if sprite’s native direction is LEFT
    private const bool nativeFacesLeft = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(BossAI());
    }
    void Chase()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * 5f, rb.linearVelocity.y);

        bool faceLeft = dir.x < 0;

        // If the sprite was drawn facing left natively, we XOR the logic:
        spriteRenderer.flipX = nativeFacesLeft ? !faceLeft : faceLeft;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BossAI()
    {
        
        yield return new WaitUntil(() => isAggro); // Optional startup delay
        updateHPBar.Enable();
        yield return StartCoroutine(Divebomb());
        // yield return new WaitUntil(() => isAggro);
        // aggro.Invoke();
        while (damageable.CurrentHealth > 0)
        {
            float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            spriteRenderer.flipX = -direction.x < 0;

            if (distance > 3f)
            {
                Chase();
                yield return new WaitForSeconds(0.1f); // Smooth movement step
            }
            else if (distance <= 2f)
            {
                yield return TailPierce();
            }
            else
            {
                // Stop chasing animation
                // if (isChasing)
                // {
                //     isChasing = false;
                //     if (runAnimCoroutine != null)
                //         StopCoroutine(runAnimCoroutine);
                // }

                rb.linearVelocity = Vector2.zero;

                int value = Random.Range(1, 3); // 1 or 2
                if (value == 1)
                {
                    yield return Divebomb();
                }
                else if (value == 2)
                {
                    yield return ClawSweep();
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator JumpBack()
    {
        Vector3 dir = -(player.transform.position - transform.position).normalized;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(dir.x * 10, 1);
        yield return new WaitForSeconds(1);
        rb.gravityScale = 1;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);
    }
    IEnumerator Divebomb()
    {
        // I BELIEVE I CAN FLY
        spriteRenderer.sprite = diveBombSprites[0];
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        Vector2 flightDirection = new Vector2(-dirToPlayer.x, 3);
        rb.gravityScale = 0;
        rb.linearVelocity = flightDirection;
        yield return new WaitForSeconds(3);
        rb.linearVelocity = Vector2.zero;
        spriteRenderer.sprite = diveBombSprites[1];
        dirToPlayer = (player.transform.position - transform.position);
        spriteRenderer.flipX = -dirToPlayer.x < 0;

        flightDirection = new Vector2(dirToPlayer.x, dirToPlayer.y);
        rb.linearVelocity = flightDirection;
        divebombHitbox.ResetHit();
        divebombHitbox.isHitboxActive = true;
        // Turn on hitboxes here
        yield return new WaitForSeconds(1.5f);
        spriteRenderer.sprite = idle;
        // Turn off hitboxes here
        divebombHitbox.isHitboxActive = false;
        rb.gravityScale = 1;
    }

    IEnumerator ClawSweep()
    {
        spriteRenderer.sprite = clawSweepSprites[0];
        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = clawSweepSprites[1];

        // activate hitboxes here
        if (spriteRenderer.flipX)
        {
            clawSweepHitboxR.ResetHit();
            clawSweepHitboxR.isHitboxActive = true;
        }
        else
        {
            clawSweepHitboxL.ResetHit();
            clawSweepHitboxL.isHitboxActive = true;
        }
        // yield return new WaitForSeconds(0.25f);

        
        yield return new WaitForSeconds(0.25f);
        clawSweepHitboxL.isHitboxActive = false;
        clawSweepHitboxR.isHitboxActive = false;
        spriteRenderer.sprite = idle;
    }

    IEnumerator TailPierce()
    {
        spriteRenderer.sprite = tailPierceSprites[0];
        yield return new WaitForSeconds(1.5f);
        spriteRenderer.sprite = tailPierceSprites[1];
        if (spriteRenderer.flipX)
        {
            tailJabHitboxR.ResetHit();
            tailJabHitboxR.isHitboxActive = true;
        }
        else
        {
            tailJabHitboxL.ResetHit();
            tailJabHitboxL.isHitboxActive = true;
        }
        yield return new WaitForSeconds(0.25f);

        tailJabHitboxL.isHitboxActive = false;
        tailJabHitboxR.isHitboxActive = false;
        spriteRenderer.sprite = idle;
    }
}
