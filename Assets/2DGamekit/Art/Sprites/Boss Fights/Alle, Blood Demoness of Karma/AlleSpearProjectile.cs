using UnityEngine;
using System.Collections;
public class AlleSpearProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D rb;
    public Hitbox hitbox;

    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        // hitbox = GetComponent<Hitbox>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AttackTarget(GameObject target)
    {
        Debug.Log("here");
        // 
        // Calculate direction from this object to the target
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation around the Z-axis
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        rb.linearVelocity = direction * 10;
        hitbox.ResetHit();
        hitbox.isHitboxActive = true;
        StartCoroutine(WaitUntilDeactivate());

    }

    IEnumerator WaitUntilDeactivate()
    {
        yield return new WaitForSeconds(3);
        hitbox.isHitboxActive = false;
        Destroy(this.gameObject);
    }
}
