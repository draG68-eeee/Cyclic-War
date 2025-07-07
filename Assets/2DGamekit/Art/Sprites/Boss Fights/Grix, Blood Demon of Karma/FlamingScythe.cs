using UnityEngine;
using Gamekit2D;
using System.Collections;
public class FlamingScythe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private GameObject player;
    private Damageable damageable;
    private Hitbox hitbox;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        damageable = player.GetComponent<Damageable>();
        hitbox = GetComponent<Hitbox>();
        StartCoroutine(ChasePlayer());
        transform.Translate(0, -0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-Vector3.forward * 5);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.linearVelocity = rb.linearVelocity * -4;
            // rb.linearVelocity = 
        }
    }
    IEnumerator ChasePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * 5, 0);
        hitbox.ResetHit();
        hitbox.isHitboxActive = true;
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }


}
