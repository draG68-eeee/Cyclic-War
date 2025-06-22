// using UnityEngine;
// using System.Collections;
// using GAmekit2D;
// public class PaleDragonAI : MonoBehaviour
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     public GameObject player;
//     public SpriteRenderer spriteRenderer;
//     public Rigidbody rb;
//     public Damageable damageable;
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         spriteRenderer = GetComponent<SpriteRenderer>();
//     }
//     void Chase()
//     {
//         Vector3 direction = (player.transform.position - transform.position).normalized;
//         rb.linearVelocity = new Vector2(direction.x * 5f, rb.linearVelocity.y);
//         spriteRenderer.flipX = direction.x < 0;

//         // Start running animation if not already
//         // if (!isChasing)
//         // {
//         //     isChasing = true;
//         //     runAnimCoroutine = StartCoroutine(RunAnimation());
//         // }
//     }
//     // Update is called once per frame
//     void Update()
//     {

//     }
    
//     IEnumerator BossAI()
//     {
//         yield return new WaitForSeconds(1f); // Optional startup delay
//         // yield return new WaitUntil(() => isAggro);
//         // aggro.Invoke();
//         while (damageable.CurrentHealth > 0)
//         {
//             float distance = Mathf.Abs(player.transform.position.x - transform.position.x);
//             Vector3 direction = (player.transform.position - transform.position).normalized;
//             spriteRenderer.flipX = direction.x < 0;

//             if (distance > 3f)
//             {
//                 Chase();
//                 yield return new WaitForSeconds(0.1f); // Smooth movement step
//             }
//             // else
//             // {
//             //     // Stop chasing animation
//             //     if (isChasing)
//             //     {
//             //         isChasing = false;
//             //         if (runAnimCoroutine != null)
//             //             StopCoroutine(runAnimCoroutine);
//             //     }

//             //     rb.linearVelocity = Vector2.zero;

//             //     int value = Random.Range(1, 3); // 1 or 2
//             //     if (value == 1)
//             //     {
//             //         yield return Attack();
//             //     }
//             //     else if (value == 2)
//             //     {
//             //         yield return OverheadSlam();
//             //     }

//             //     yield return new WaitForSeconds(0.5f);
//             // }
//         }
//     }
// }
