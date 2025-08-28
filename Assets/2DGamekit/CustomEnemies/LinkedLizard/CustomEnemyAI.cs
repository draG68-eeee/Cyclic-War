using UnityEngine;
using System.Collections;
using System;
public class CustomEnemyAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite[] clawSwipe;
    public Sprite idle;
    public Hitbox clawSwipeHitboxL;
    public Hitbox clawSwipeHitboxR;
    private SpriteRenderer spriteRenderer;
    public GameObject aggroRange;
    public bool isAggro = false;
    private GameObject player;
    private Rigidbody2D rb;
    private Coroutine coroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        StartCoroutine(CallEnemyAI());
    }

    IEnumerator CallEnemyAI()
    {
        while (true)
        {
            yield return EnemyAI();

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    
    IEnumerator EnemyAI()
    {
        yield return new WaitUntil(() => isAggro == true);
        while (isAggro)
        {
            yield return ChasePlayer(player);
            yield return ClawSwipe();
        }
        
    }

    IEnumerator ChasePlayer(GameObject player)
    {
        while (Mathf.Abs(player.transform.position.x - transform.position.x) >= 2)
        {
            // Only chase horizontally
            Vector3 target = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
            Vector3 dir = (target - transform.position).normalized;

            // Flip sprite based on direction
            spriteRenderer.flipX = player.transform.position.x >= transform.position.x;

            // Apply horizontal movement only, keep current Y velocity (gravity)
            rb.linearVelocity = new Vector2(dir.x * 3, rb.linearVelocity.y);

            yield return new WaitForSeconds(0.1f); // Small delay to avoid busy loop
        }

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop moving horizontally once close enough
    }


    IEnumerator ClawSwipe()
    {
        if (player.transform.position.x < transform.position.x) // facing left
        {
            spriteRenderer.flipX = false;
            spriteRenderer.sprite = clawSwipe[0];
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.sprite = clawSwipe[1];
            clawSwipeHitboxL.ResetHit();
            clawSwipeHitboxL.isHitboxActive = true;
            yield return new WaitForSeconds(0.2f);
            clawSwipeHitboxL.isHitboxActive = false;
        }
        else // facing right
        {
            spriteRenderer.flipX = true;
            spriteRenderer.sprite = clawSwipe[0];
            yield return new WaitForSeconds(0.5f);
            spriteRenderer.sprite = clawSwipe[1];
            clawSwipeHitboxR.ResetHit();
            clawSwipeHitboxR.isHitboxActive = true;
            yield return new WaitForSeconds(0.2f);
            clawSwipeHitboxR.isHitboxActive = false;
        }
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = idle;
    }


    
}
