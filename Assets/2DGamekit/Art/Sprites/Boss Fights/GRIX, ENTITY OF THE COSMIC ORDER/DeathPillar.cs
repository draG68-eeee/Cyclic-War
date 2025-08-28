using UnityEngine;
using System.Collections;
public class DeathPillar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private Hitbox hitbox;
    void Start()
    {
        hitbox = GetComponent<Hitbox>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AttackPlayer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AttackPlayer()
    {
        spriteRenderer.sprite = sprites[0];
        yield return new WaitForSeconds(1);
        spriteRenderer.sprite = sprites[1];
        hitbox.ResetHit();
        hitbox.isHitboxActive = true;
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
