using UnityEngine;
using System.Collections;
using Gamekit2D;
using UnityEngine.SceneManagement;
public class EntityAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject panel;
    private Coroutine coroutine;
    private Damageable damageable;
    private AudioSource audioSource;
    public UpdateHPBar updateHPBar;
    public GameObject player;
    public Damageable playerDamageable;
    // projectiles
    public GameObject deathOrb;
    public GameObject deathPillar;
    // hitboxes
    public Hitbox scytheHitboxL;
    public Hitbox scytheHitboxR;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    // Sprites/Combo moves

    public Sprite[] ScytheComboSprites;
    public Sprite[] KILLSTRIKE;
    public Sprite idle;

    void Awake()
    {
        // updateHPBar = GetComponent<UpdateHPBar>();
        // updateHPBar.Enable();
        damageable = GetComponent<Damageable>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        
        
        // coroutine = StartCoroutine(EntityBossAI());
        // StartCoroutine(WaitUntilDeath());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // IEnumerator WaitUntilDeath()
    // {
    //     yield return new WaitUntil(() => damageable.CurrentHealth <= 0);
    //     updateHPBar.Disable();
    //     audioSource.Stop();
    //     StopCoroutine(coroutine);
    // }

    void TeleportFromPlayer(float value)
    {
        int number = Random.Range(1, 3);
        if (number == 1)
        {
            transform.position = new Vector2(player.transform.position.x - value, player.transform.position.y + 2);
        }
        else if (number == 2)
        {
            transform.position = new Vector2(player.transform.position.x + value, player.transform.position.y + 2);
        }
        Vector3 direction = (player.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
    }
    public IEnumerator EntityBossAI()
    {

        // panel.SetActive(true);
        audioSource.Play();
        updateHPBar.Enable();
        yield return DeathOrbSummon();
        while (true)
        {
            if (damageable.CurrentHealth < 100)
            {
                yield return KillPlayer();
            }
            int move = Random.Range(1, 4);
            if (move == 1)
            {
                yield return ScytheComboAttack();
            }
            else if (move == 2)
            {
                yield return DeathOrbSummon();
            }
            else if (move == 3)
            {
                yield return DeathPillarSummon();
            }

        }
        
        
    }

    IEnumerator KillPlayer()
    {
        TeleportFromPlayer(2);
        spriteRenderer.sprite = KILLSTRIKE[0];
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = KILLSTRIKE[1];
        playerDamageable.SetHealth(1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Ending 2 scene");

    }

    IEnumerator DeathOrbSummon()
    {
        spriteRenderer.sprite = idle;
        TeleportFromPlayer(8);
        transform.position += new Vector3(0, 3, 0);
        Instantiate(deathOrb, new Vector3(transform.position.x + 4, transform.position.y + 5, 1), transform.rotation);
        Instantiate(deathOrb, new Vector3(transform.position.x - 4, transform.position.y + 5, 1), transform.rotation);
        Instantiate(deathOrb, new Vector3(transform.position.x, transform.position.y + 5, 1), transform.rotation);
        yield return new WaitForSeconds(5);
    }

    IEnumerator DeathPillarSummon()
    {
        spriteRenderer.sprite = idle;
        TeleportFromPlayer(8);
        transform.position += new Vector3(0, 3, 0);
        Instantiate(deathPillar, new Vector3(transform.position.x - 35, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x - 28, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x - 21, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x - 14, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x - 7, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x + 7, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x + 14, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x + 21, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x + 28, transform.position.y + 2, 1), transform.rotation);
        Instantiate(deathPillar, new Vector3(transform.position.x + 35, transform.position.y + 2, 1), transform.rotation);
        yield return new WaitForSeconds(5);
    }

    IEnumerator ScytheComboAttack()
    {
        TeleportFromPlayer(2);
        

        spriteRenderer.sprite = ScytheComboSprites[0];
        yield return new WaitForSeconds(0.6f);
        spriteRenderer.sprite = ScytheComboSprites[1];

        // activate hitbox
        if (spriteRenderer.flipX)
        {
            scytheHitboxL.ResetHit();
            scytheHitboxL.isHitboxActive = true;
        }
        else
        {
            scytheHitboxR.ResetHit();
            scytheHitboxR.isHitboxActive = true;
        }
        yield return new WaitForSeconds(0.3f);

        scytheHitboxL.isHitboxActive = false;
        scytheHitboxR.isHitboxActive = false;


        spriteRenderer.sprite = ScytheComboSprites[2];
        yield return new WaitForSeconds(0.6f);
        spriteRenderer.sprite = ScytheComboSprites[3];
        // activate hitbox
        if (spriteRenderer.flipX)
        {
            scytheHitboxL.ResetHit();
            scytheHitboxL.isHitboxActive = true;
        }
        else
        {
            scytheHitboxR.ResetHit();
            scytheHitboxR.isHitboxActive = true;
        }
        yield return new WaitForSeconds(2);


    }
}
