using UnityEngine;
using Gamekit2D;
public class NPCAllyHitbox : MonoBehaviour
{
    public bool isHitboxActive = false;
    private bool hasHit = false;
    public int damageValue;


    void Start()
    {

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (!isHitboxActive || hasHit || !other.CompareTag("Enemy"))
            return;

        Parry parry = other.gameObject.GetComponent<Parry>();

        
    
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable != null)
        {
                // if (damageValue == 1)
                //     // Debug.Log("YIPPIE I HIT JOHN DARK SOULS!");
                // else if (damageValue == 2)
                //     // Debug.Log("YIPPIE I GAVE JOHN DARK SOULS A CONCUSSION! HE DONT BE GETTING UP ANYTIME SOON!");

            damageable.SetHealth(damageable.CurrentHealth - damageValue);
            // Debug.Log($"Player Health: {damageable.CurrentHealth}");
        }
        else
        {
            Debug.LogWarning("Hit something with no Damageable component.");
        }
        

        hasHit = true;
    }

    public void ResetHit()
    {
        hasHit = false;
    }
}
