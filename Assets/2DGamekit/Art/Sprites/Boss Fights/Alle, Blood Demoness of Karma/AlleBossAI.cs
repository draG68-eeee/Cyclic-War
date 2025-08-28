using UnityEngine;
using UnityEngine.Events;
using Gamekit2D;
using System.Collections;
public class AlleBossAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float groundY;
    public GameObject victim;
    public Rigidbody2D rb;
    public Damageable damageable;
    public UpdateHPBar updateHPBar;
    public GameObject[] targets;
    public SpriteRenderer spriteRenderer;
    public Sprite[] DashThrust;
    public Sprite[] DoubleThrust;
    public Sprite[] SpearThrow;
    public Sprite[] SickleCombo;
    public Sprite idle;
    // hitboxes
    public Hitbox doubleThrustR;
    public Hitbox doubleThrustL;
    public Hitbox dashSlashHitbox;
    // projectiles

    public GameObject spearProjectile;
    public AlleSpearProjectile spearProjectileFunction;
    public int currentAttack;

    private Coroutine coroutine;
    public UnityEvent BeginFight;


    void Awake()
    {
        coroutine = StartCoroutine(MovesetAI());
        // spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        // damageable = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (damageable.CurrentHealth <= 0)
        {
            StopCoroutine(coroutine);
            spriteRenderer.sprite = idle;
            rb.linearVelocity = new Vector2(0, 1);
        }
    }

    IEnumerator MovesetAI()
    {
        yield return new WaitForSeconds(5);
        // yield return SickleComboAttack();
        updateHPBar.Enable();
        BeginFight.Invoke();
        victim = targets[1];
        yield return SpearThrowAttack();
        while (true)
        {
            int number = Random.Range(1, 4);
            if (number == 1)
            {
                yield return DashThrustAttack();
            }
            else if (number == 2)
            {
                yield return SpearThrowAttack();
            }
            else if (number == 3)
            {
                yield return DoubleThrustAttack();
            }

        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator DashThrustAttack()
    {
        currentAttack = 1;
        // GameObject victim;
        int p = Random.Range(1, 3);
        switch (p)
        {
            case 1:
                victim = targets[0];
                break;
            case 2:
                victim = targets[1];
                break;
            default:
                victim = targets[1];
                break;
        }
        p = Random.Range(1, 3);
        int dir;
        if (p == 1)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        spriteRenderer.sprite = DashThrust[0];
        transform.position = new Vector3(victim.transform.position.x + dir * 7, groundY, 1);

        Vector3 direction = (victim.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;

        yield return new WaitForSeconds(2);
        spriteRenderer.sprite = DashThrust[1];
        // Vector3 launchDirection = (victim.transform.position - transform.position).normalized;
        yield return TeleportBehindPlayer(direction.x);
        yield return new WaitForSeconds(3);
        spriteRenderer.sprite = idle;

    }

    IEnumerator SpearThrowAttack()
    {
        currentAttack = 2;
        // GameObject victim;
        int p = Random.Range(1, 3);
        switch (p)
        {
            case 1:
                victim = targets[0];
                break;
            case 2:
                victim = targets[1];
                break;
            default:
                victim = targets[1];
                break;
        }
        int deltaX = Random.Range(-7, 7);
        int deltaY = 6;
        transform.position = new Vector3(victim.transform.position.x + deltaX, groundY + deltaY, 1);
        Vector3 direction = (victim.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
        // begin the throwing animation
        spriteRenderer.sprite = SpearThrow[0];
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = SpearThrow[1];
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = SpearThrow[2];
        // instantiate spear object here
        GameObject spear = Instantiate(spearProjectile, transform.position, transform.rotation);
        AlleSpearProjectile asp = spear.GetComponent<AlleSpearProjectile>();
        asp.AttackTarget(victim);
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.sprite = idle;
        yield return new WaitForSeconds(2);
    }

    IEnumerator DoubleThrustAttack()
    {
        // float direction = ()
        currentAttack = 3;
        // GameObject victim;
        int p = Random.Range(1, 3);
        switch (p)
        {
            case 1:
                victim = targets[0];
                break;
            case 2:
                victim = targets[1];
                break;
            default:
                victim = targets[1];
                break;
        }
        p = Random.Range(1, 3);
        int dir;
        if (p == 1)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        // orient boss
        // spriteRenderer.sprite = DashThrust[0];
        transform.position = new Vector3(victim.transform.position.x + dir * 3, groundY - 1, 1);

        Vector3 direction = (victim.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
        // animation
        spriteRenderer.sprite = DoubleThrust[0];
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = DoubleThrust[1];
        // activate hitboxes
        if (spriteRenderer.flipX)
        {
            doubleThrustL.ResetHit();
            doubleThrustL.isHitboxActive = true;
        }
        else
        {
            doubleThrustR.ResetHit();
            doubleThrustR.isHitboxActive = true;
        }
        yield return new WaitForSeconds(0.3f);
        doubleThrustL.isHitboxActive = false;
        doubleThrustR.isHitboxActive = false;
        spriteRenderer.sprite = DoubleThrust[2];
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = DoubleThrust[3];
        if (spriteRenderer.flipX)
        {
            doubleThrustL.ResetHit();
            doubleThrustL.isHitboxActive = true;
        }
        else
        {
            doubleThrustR.ResetHit();
            doubleThrustR.isHitboxActive = true;
        }
        rb.linearVelocity = new Vector2(direction.x * 4, 0);
        yield return new WaitForSeconds(0.3f);
        doubleThrustL.isHitboxActive = false;
        doubleThrustR.isHitboxActive = false;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(2);

    }

    IEnumerator SickleComboAttack()
    {
        currentAttack = 4;
        // GameObject victim;
        int p = Random.Range(1, 3);
        switch (p)
        {
            case 1:
                victim = targets[0];
                break;
            case 2:
                victim = targets[1];
                break;
            default:
                victim = targets[1];
                break;
        }
        int i = 0;
        while (i < 8)
        {
            int x = Random.Range(1, 3);
            if (x == 1)
            {
                transform.position = new Vector3(victim.transform.position.x - 3, victim.transform.position.y, 1);
                spriteRenderer.flipX = false;
            }
            else
            {
                transform.position = new Vector3(victim.transform.position.x + 3, victim.transform.position.y, 1);
                spriteRenderer.flipX = true;
            }
            spriteRenderer.sprite = SickleCombo[i];
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.sprite = SickleCombo[i + 1];
            yield return new WaitForSeconds(0.3f);
            
            i += 2;
        }
        
    }

    // Teleport functions
    IEnumerator TeleportBehindPlayer(float xDirection)
    {
        dashSlashHitbox.ResetHit();
        dashSlashHitbox.isHitboxActive = true;
        if (xDirection > 0)
        {
            rb.linearVelocity = new Vector2(100, 0);
            yield return new WaitForSeconds(0.2f);
            rb.linearVelocity = Vector2.zero;
        }
        else if (xDirection < 0)
        {
            rb.linearVelocity = new Vector2(-100, 0);
            yield return new WaitForSeconds(0.2f);
            rb.linearVelocity = Vector2.zero;
        }
        dashSlashHitbox.isHitboxActive = false;
    }
}
