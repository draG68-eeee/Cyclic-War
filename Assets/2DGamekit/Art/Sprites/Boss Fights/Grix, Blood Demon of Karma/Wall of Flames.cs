using UnityEngine;
using System.Collections;
public class WallofFlames : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite[] animation;
    private SpriteRenderer spriteRenderer;
    public Hitbox hitbox;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox = GetComponent<Hitbox>();
        StartCoroutine(Rise());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Rise()
    {
        
        spriteRenderer.sprite = animation[0];
        yield return new WaitForSeconds(2);
        spriteRenderer.sprite = animation[1];
        hitbox.ResetHit();
        hitbox.isHitboxActive = true;
        yield return new WaitForSeconds(1);
        hitbox.isHitboxActive = false;
        Destroy(this.gameObject);
    }
}
