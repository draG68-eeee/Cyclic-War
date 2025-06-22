using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Gamekit2D;
public class GolemKnightAI : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;
    public Damageable damageable;
    public Damageable selfDamageable;
    public Sprite idleSprite;
    public Sprite[] windupSprites;
    public Sprite[] finisherSprites;
    public Sprite[] runningAnimations;

    public Hitbox hitboxR;
    public Hitbox hitboxL;
    public Hitbox overheadHitboxR;
    public Hitbox overheadHitboxL;
    public bool isAggro = false;
    private AudioSource audioSource;
    private Coroutine runAnimCoroutine;
    private bool isChasing = false;
    public UnityEvent aggro;
    public UnityEvent victory;
    public enum BossState
    {
        Idle,
        Chase,
        Attack,
        PhaseTransition,
        Dead
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BossAI());
        audioSource = GetComponent<AudioSource>();
    }

    void Chase()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * 5f, rb.linearVelocity.y);
        spriteRenderer.flipX = direction.x < 0;

        // Start running animation if not already
        if (!isChasing)
        {
            isChasing = true;
            runAnimCoroutine = StartCoroutine(RunAnimation());
        }
    }
    void Update()
    {
        if (damageable.CurrentHealth <= 0)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
    IEnumerator RunAnimation()
    {
        int index = 0;
        while (true)
        {
            spriteRenderer.sprite = runningAnimations[index];
            index = (index + 1) % runningAnimations.Length;
            yield return new WaitForSeconds(0.1f); // adjust speed as needed
        }
    }

    IEnumerator BossAI()
    {
        yield return new WaitForSeconds(1f); // Optional startup delay
        yield return new WaitUntil(() => isAggro);
        aggro.Invoke();
        while (damageable.CurrentHealth > 0)
        {
            float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            spriteRenderer.flipX = direction.x < 0;

            if (distance > 3f)
            {
                Chase();
                yield return new WaitForSeconds(0.1f); // Smooth movement step
            }
            else
            {
                // Stop chasing animation
                if (isChasing)
                {
                    isChasing = false;
                    if (runAnimCoroutine != null)
                        StopCoroutine(runAnimCoroutine);
                }

                rb.linearVelocity = Vector2.zero;

                int value = Random.Range(1, 3); // 1 or 2
                if (value == 1)
                {
                    yield return Attack();
                }
                else if (value == 2)
                {
                    yield return OverheadSlam();
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
        StartCoroutine(BossAI());
    }

    IEnumerator Attack()
    {
        spriteRenderer.sprite = windupSprites[0];
        yield return new WaitForSeconds(1f);

        spriteRenderer.sprite = finisherSprites[0];

        if (spriteRenderer.flipX)
        {
            hitboxL.ResetHit();
            hitboxL.isHitboxActive = true;
        }
        else
        {
            hitboxR.ResetHit();
            hitboxR.isHitboxActive = true;
        }

        yield return new WaitForSeconds(0.2f);

        hitboxR.isHitboxActive = false;
        hitboxL.isHitboxActive = false;

        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = idleSprite;
    }

    IEnumerator OverheadSlam()
    {
        spriteRenderer.sprite = windupSprites[1];
        audioSource.Play(0);
        yield return new WaitForSeconds(1f);

        spriteRenderer.sprite = finisherSprites[1];

        if (spriteRenderer.flipX)
        {
            overheadHitboxL.ResetHit();
            overheadHitboxL.isHitboxActive = true;
        }
        else
        {
            overheadHitboxR.ResetHit();
            overheadHitboxR.isHitboxActive = true;
        }

        yield return new WaitForSeconds(0.2f);

        overheadHitboxR.isHitboxActive = false;
        overheadHitboxL.isHitboxActive = false;

        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = idleSprite;
    }

    IEnumerator Backstep()
    {
        Vector3 direction = -(player.transform.position - transform.position).normalized * 10;
        rb.linearVelocity = new Vector2(direction.x, 0);
        yield return new WaitForSeconds(1f);
        rb.linearVelocity = Vector2.zero;
    }
}
