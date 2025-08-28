using UnityEngine;
using System.Collections;


public class DeathOrb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Hitbox hitbox;
    private Rigidbody2D rb;
    private GameObject player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Hitbox>();
        hitbox.ResetHit();
        hitbox.isHitboxActive = true;
        player = GameObject.Find("Player");
        StartCoroutine(AttackPlayer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 dir = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * 6, dir.y * 6);
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
