using UnityEngine;
using Gamekit2D;
public class Hitbox : MonoBehaviour
{
    public bool isHitboxActive = false;
    private bool hasHit = false;
    public int damageValue;
    public EnergyGauge energyGauge;
    public GameObject energyGaugeUI;

    void Start()
    {
        energyGaugeUI = GameObject.Find("EnergyGauge");
        energyGauge = energyGaugeUI.GetComponent<EnergyGauge>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (!isHitboxActive || hasHit || !other.CompareTag("Player"))
            return;

        Parry parry = other.gameObject.GetComponent<Parry>();
        if (parry != null && parry.isInvincible)
        {
            parry.ParrySuccess();              // <-- add this
            energyGauge.UpdateEnergy(25);
            hasHit = true;                     // (optional) mark consumed
            return;                            // (optional) be explicit
        }

        else
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                // if (damageValue == 1)
                //     // Debug.Log("YIPPIE I HIT JOHN DARK SOULS!");
                // else if (damageValue == 2)
                //     // Debug.Log("YIPPIE I GAVE JOHN DARK SOULS A CONCUSSION! HE DONT BE GETTING UP ANYTIME SOON!");

                damageable.SetHealth(damageable.CurrentHealth - damageValue);
                Debug.Log($"Player Health: {damageable.CurrentHealth}");
            }
            else
            {
                Debug.LogWarning("Hit something with no Damageable component.");
            }
        }

        hasHit = true;
    }

    public void ResetHit()
    {
        hasHit = false;
    }
}












// using UnityEngine;
// using Gamekit2D;

// public class Hitbox : MonoBehaviour
// {
//     [Header("State")]
//     public bool isHitboxActive = false;    // toggled by animation events
//     private bool hasHit = false;

//     [Header("Damage")]
//     public int damageValue = 1;

//     [Header("Energy")]
//     public EnergyGauge energyGauge;        // assign in Inspector if possible
//     public GameObject energyGaugeUI;       // optional: only if you don't assign above

//     private void Awake()
//     {
//         // If not set in Inspector, try to find once.
//         if (energyGauge == null)
//         {
//             if (energyGaugeUI == null) energyGaugeUI = GameObject.Find("EnergyGauge");
//             if (energyGaugeUI != null) energyGauge = energyGaugeUI.GetComponent<EnergyGauge>();
//         }
//     }

//     private void OnEnable()
//     {
//         // safety: always reset per enable / spawn
//         hasHit = false;
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (!isHitboxActive || hasHit || !other.CompareTag("Player"))
//             return;

//         // Try parry first
//         Parry parry = other.GetComponent<Parry>();
//         if (parry != null && parry.IsParryActive)
//         {
//             // Signal a TRUE parry so the player gets short recovery; discourages spam.
//             parry.OnParrySuccess();

//             if (energyGauge != null)
//                 energyGauge.UpdateEnergy(25);

//             // Optional: recoil this attacker, play spark FX, etc.
//             hasHit = true;
//             return;
//         }

//         // Otherwise, apply damage normally
//         Damageable damageable = other.GetComponent<Damageable>();
//         if (damageable != null)
//         {
//             damageable.SetHealth(damageable.CurrentHealth - damageValue);
//             Debug.Log($"Player Health: {damageable.CurrentHealth}");
//         }
//         else
//         {
//             Debug.LogWarning("Hit something with no Damageable component.");
//         }

//         hasHit = true;
//     }

//     // Call this from animation events at the start of each swing/active frames
//     public void ActivateHitbox()
//     {
//         isHitboxActive = true;
//         hasHit = false; // reset per swing so we can hit once per attack
//     }

//     // Call this from animation events at the end of active frames
//     public void DeactivateHitbox()
//     {
//         isHitboxActive = false;
//     }

//     // Kept for compatibility if you already call this from somewhere else
//     public void ResetHit()
//     {
//         hasHit = false;
//     }
// }
