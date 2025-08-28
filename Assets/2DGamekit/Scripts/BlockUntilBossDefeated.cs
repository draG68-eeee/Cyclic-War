using UnityEngine;
using Gamekit2D;
using System.Collections;
public class BlockUntilBossDefeated : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Damageable damageable;
    void Start()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Wait()
    {
        yield return new WaitUntil(() => damageable.CurrentHealth <= 0);
        this.gameObject.SetActive(false);
    }
}
