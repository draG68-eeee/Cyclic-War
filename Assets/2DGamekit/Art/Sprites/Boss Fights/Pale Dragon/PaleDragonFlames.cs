using UnityEngine;
using System.Collections;
public class PaleDragonFlames : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject paleDragon;
    private PaleDragonAI paleDragonAI;
    private string direction;
    private Rigidbody2D rb;
    private Hitbox hitbox;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        paleDragon = GameObject.Find("Pale Dragon");
        hitbox = GetComponent<Hitbox>();
        paleDragonAI = paleDragon.GetComponent<PaleDragonAI>();
        direction = paleDragonAI.flameDirection;
        hitbox.ResetHit();
        hitbox.isHitboxActive = true;
        StartCoroutine(WaitToDestroy());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
    
    
}
