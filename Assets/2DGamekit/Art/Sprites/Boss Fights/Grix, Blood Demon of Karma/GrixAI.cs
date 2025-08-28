using UnityEngine;
using System.Collections;
using Gamekit2D;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GrixAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EllaQuestlineHandler ellaQuestlineHandler;
    public GameObject Alle;
    public GameObject TRUEFORMGRIX;
    public EntityAI entityAI;
    public Sprite ending2Dialogue;
    public Sprite YOUCANNOTESCAPE;
    public Sprite[] cutscene;
    public Image specialDialogue;
    public Sprite idle;

    public Sprite[] crossCombo;
    public Sprite[] dashSlash;
    public Sprite[] scytheCombo;
    public Sprite[] scytheThrow;
    public Sprite[] flameWallAnimation;
    public SpriteRenderer spriteRenderer;
    public GameObject player;

    private Coroutine coroutine;
    public Rigidbody2D rb;

    public Hitbox[] crossSlashR;
    public Hitbox[] crossSlashL;
    public Hitbox[] scytheComboHitboxesL;
    public Hitbox[] scytheComboHitboxesR;
    public Hitbox dashSlashHitbox;
    public UpdateHPBar updateHPBar;
    public Damageable damageable;
    public bool isAggro;
    // Environmental hazards
    public GameObject FlamingScythe;
    public GameObject WallOfFlames;
    public GrixMusicPlayer musicPlayer; // Reference to the music player
    public AudioClip outro;
    void Start()
    {
        coroutine = StartCoroutine(BossAI());
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
        StartCoroutine(OnDeath());
        // updateHPBar = GetComponent<UpdateHPBar>();
        if (musicPlayer == null)
        {
            musicPlayer = FindObjectOfType<GrixMusicPlayer>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator OnDeath()
    {
        yield return new WaitUntil(() => damageable.CurrentHealth <= 0);
        StopCoroutine(coroutine);
        spriteRenderer.sprite = idle;
        updateHPBar.Disable();
        // dashSlashHitbox.isHitboxActive = false;
        if (musicPlayer != null && musicPlayer.audioSource != null)
        {
            musicPlayer.audioSource.Stop();
            musicPlayer.audioSource.clip = outro;
            musicPlayer.audioSource.Play();
            rb.linearVelocity = new Vector2(0, 1);
            yield return new WaitForSeconds(20);
            SceneManager.LoadScene("Ending 1 scene");
        }
    }

    IEnumerator StartCutscene()
    {
        specialDialogue.gameObject.SetActive(true);
        specialDialogue.sprite = cutscene[0];
        yield return new WaitForSeconds(5);
        specialDialogue.sprite = cutscene[1];
        yield return new WaitForSeconds(5);
        specialDialogue.sprite = cutscene[2];
        updateHPBar.Enable();
        if (musicPlayer != null)
        {
            musicPlayer.PlayIntro();
            // Wait for intro to finish
            yield return new WaitForSeconds(3);
            specialDialogue.gameObject.SetActive(false);
            yield return new WaitUntil(() => !musicPlayer.IsPlaying());
            musicPlayer.PlayPhase1();
        }
    
    }

    IEnumerator StartSecondEndingCutscene()
    {
        specialDialogue.gameObject.SetActive(true);
        specialDialogue.sprite = ending2Dialogue;
        yield return new WaitForSeconds(1);
        specialDialogue.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Alle.SetActive(true);
        Damageable alleBoss = Alle.GetComponent<Damageable>();
        if (alleBoss)
        {
            yield return new WaitUntil(() => alleBoss.CurrentHealth <= 0);
            specialDialogue.gameObject.SetActive(true);
            specialDialogue.sprite = YOUCANNOTESCAPE;
            yield return new WaitForSeconds(1);
            specialDialogue.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            StartCoroutine(entityAI.EntityBossAI());
            
        }
        

    }

    IEnumerator BossAI()
    {
        yield return new WaitUntil(() => isAggro);
        if (CheckpointManager.Instance.ellaQuestlinePhase == 4)
        {
            yield return StartSecondEndingCutscene();
            yield break;
        }
        yield return StartCutscene();
        // updateHPBar.Enable();
        // if (musicPlayer != null)
        // {
        //     musicPlayer.PlayIntro();
        //     // Wait for intro to finish
        //     yield return new WaitUntil(() => !musicPlayer.IsPlaying());
        //     musicPlayer.PlayPhase1();
        // }
        yield return ScytheCombo();
        bool phase2Started = false;
        while (true)
        {
            float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            spriteRenderer.flipX = direction.x < 0;

            // Switch to phase 2 music if health drops below half
            if (!phase2Started && damageable.CurrentHealth <= (int)damageable.startingHealth / 2)
            {
                if (musicPlayer != null)
                {
                    musicPlayer.PlayPhase2();
                }
                phase2Started = true;
            }

            if (damageable.CurrentHealth > (int)damageable.startingHealth / 2)
            {
                int value = Random.Range(1, 5);
                if (value == 1)
                {
                    yield return CrossCombo();
                }
                else if (value == 2)
                {
                    yield return Dash();
                }
                else if (value == 3)
                {
                    yield return ScytheThrow();
                }
                else if (value == 4)
                {
                    yield return FlameWallCast();
                }
            }
            else
            {
                int value = Random.Range(1, 6);
                if (value == 1)
                {
                    yield return CrossCombo();
                }
                else if (value == 2)
                {
                    yield return Dash();
                }
                else if (value == 3)
                {
                    yield return ScytheCombo();
                }
                else if (value == 4)
                {
                    yield return ScytheThrow();
                }
                else if (value == 5)
                {
                    yield return FlameWallCast();
                }
            }
        }
    }

    IEnumerator Dash()
    {
        int number = Random.Range(1, 3);
        if (number == 1)
        {
            transform.position = new Vector2(player.transform.position.x - 10, player.transform.position.y + 3);
        }
        else if (number == 2)
        {
            transform.position = new Vector2(player.transform.position.x + 10, player.transform.position.y + 3);
        }
        Vector3 direction = (player.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;

        spriteRenderer.sprite = dashSlash[0];
        yield return new WaitForSeconds(2);
        spriteRenderer.sprite = dashSlash[1];
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = dashSlash[2];
        yield return TeleportBehindPlayer(direction.x);
        yield return new WaitForSeconds(2);


    }

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

    void TeleportNearPlayer()
    {
        int number = Random.Range(1, 3);
        if (number == 1)
        {
            transform.position = new Vector2(player.transform.position.x - 2, player.transform.position.y + 3);
        }
        else if (number == 2)
        {
            transform.position = new Vector2(player.transform.position.x + 2, player.transform.position.y + 3);
        }
        Vector3 direction = (player.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
    }
    void TeleportFromPlayer(float value)
    {
        int number = Random.Range(1, 3);
        if (number == 1)
        {
            transform.position = new Vector2(player.transform.position.x - value, player.transform.position.y + 3);
        }
        else if (number == 2)
        {
            transform.position = new Vector2(player.transform.position.x + value, player.transform.position.y + 3);
        }
        Vector3 direction = (player.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
    }

    IEnumerator CrossCombo()
    {
        // tp to player 
        int number = Random.Range(1, 3);
        if (number == 1)
        {
            transform.position = new Vector2(player.transform.position.x - 2, player.transform.position.y + 3);
        }
        else if (number == 2)
        {
            transform.position = new Vector2(player.transform.position.x + 2, player.transform.position.y + 3);
        }
        Vector3 direction = (player.transform.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
        spriteRenderer.sprite = crossCombo[0];
        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = crossCombo[1];
        // activate hitboxes

        transform.position = new Vector2(transform.position.x + direction.x * 4, transform.position.y);
        if (spriteRenderer.flipX)
        {
            crossSlashL[0].ResetHit();
            crossSlashL[0].isHitboxActive = true;
        }
        else
        {
            crossSlashR[0].ResetHit();
            crossSlashR[0].isHitboxActive = true;
        }

        yield return new WaitForSeconds(0.25f);
        crossSlashL[0].isHitboxActive = false;
        crossSlashR[0].isHitboxActive = false;
        spriteRenderer.sprite = crossCombo[2];
        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = crossCombo[3];
        // activate hitboxes
        if (spriteRenderer.flipX)
        {
            crossSlashL[1].ResetHit();
            crossSlashL[1].isHitboxActive = true;
        }
        else
        {
            crossSlashR[1].ResetHit();
            crossSlashR[1].isHitboxActive = true;
        }

        yield return new WaitForSeconds(0.25f);
        crossSlashL[1].isHitboxActive = false;
        crossSlashR[1].isHitboxActive = false;

    }

    IEnumerator ScytheCombo()
    {
        // TeleportNearPlayer();

        int i = 0;
        int hitboxNumber = 0;
        // yield return new WaitForSeconds(1);
        while (i < 8)
        {
            TeleportNearPlayer();
            spriteRenderer.sprite = scytheCombo[i];
            yield return new WaitForSeconds(0.6f);
            spriteRenderer.sprite = scytheCombo[i + 1];
            if (spriteRenderer.flipX)
            {
                scytheComboHitboxesL[hitboxNumber].ResetHit();
                scytheComboHitboxesL[hitboxNumber].isHitboxActive = true;
            }
            else
            {
                scytheComboHitboxesR[hitboxNumber].ResetHit();
                scytheComboHitboxesR[hitboxNumber].isHitboxActive = true;
            }

            yield return new WaitForSeconds(0.25f);
            scytheComboHitboxesL[hitboxNumber].isHitboxActive = false;
            scytheComboHitboxesR[hitboxNumber].isHitboxActive = false;
            // yield return new WaitForSeconds(0.25f);
            i += 2;
            hitboxNumber++;
        }
        yield return new WaitForSeconds(3);
        spriteRenderer.sprite = idle;
    }

    IEnumerator ScytheThrow()
    {
        // TeleportBehindPlayer();
        TeleportFromPlayer(6);
        spriteRenderer.sprite = scytheThrow[0];
        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = scytheThrow[1];
        yield return new WaitForSeconds(0.3f);
        Instantiate(FlamingScythe, transform.position, transform.rotation);
        yield return new WaitForSeconds(1);
    }

    IEnumerator FlameWallCast()
    {
        TeleportFromPlayer(11);
        spriteRenderer.sprite = flameWallAnimation[0];
        yield return new WaitForSeconds(2);
        spriteRenderer.sprite = flameWallAnimation[1];
        Instantiate(WallOfFlames, new Vector2(transform.position.x + 10, transform.position.y - 1.5f), transform.rotation);
        Instantiate(WallOfFlames, new Vector2(transform.position.x - 10, transform.position.y - 1.5f), transform.rotation);
        yield return new WaitForSeconds(2);
        spriteRenderer.sprite = idle;
    }
}
