using UnityEngine;
using System.Collections;
public class TitanDomainGuardianAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    public Sprite idle;
    public Sprite[] throwAtPlayer;
    public GameObject deletionBolt;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void CallChasePlayer()
    {
        StartCoroutine(ChasePlayer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ChasePlayer()
    {
        while (true)
        {
            float dx = player.transform.position.x - transform.position.x;
            float dy = player.transform.position.y - transform.position.y;
            float distance = Mathf.Sqrt(dx * dx + dy * dy);
            if (distance > 7)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                rb.linearVelocity = new Vector2(direction.x, direction.y) * speed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                yield return Attack();
            }
            Vector3 dir = player.transform.position - transform.position;
            spriteRenderer.flipX = dir.x < 0;
            yield return new WaitForSeconds(0.1f);
        }



    }

    public void Stop()
    {
        Destroy(this.gameObject);
    }

    IEnumerator Attack()
    {
        spriteRenderer.sprite = throwAtPlayer[0];
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.sprite = throwAtPlayer[1];
        GameObject bolt = Instantiate(deletionBolt, transform.position, transform.rotation);
        AlleSpearProjectile boltProjectile = bolt.GetComponent<AlleSpearProjectile>();
        boltProjectile.AttackTarget(player);
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = idle;
    }
}
