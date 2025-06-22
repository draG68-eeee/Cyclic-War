using UnityEngine;
using System.Collections;

public class Parry : MonoBehaviour
{
    public bool isInvincible { get; private set; } // public read, private write
    private bool canParry = true;

    [Header("Parry Settings")]
    public float invincibilityTime = 0.2f;
    public float parryCooldown = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && canParry)
        { 
            StartCoroutine(Deflect());
        }
    }

    IEnumerator Deflect()
    {
        canParry = false;
        isInvincible = true;
        Debug.Log("Parry active! Player is invincible.");

        yield return new WaitForSeconds(invincibilityTime);

        isInvincible = false;
        Debug.Log("Parry ended. Player vulnerable.");

        yield return new WaitForSeconds(parryCooldown - invincibilityTime);

        canParry = true;
        Debug.Log("Parry ready again.");
    }
}
