using UnityEngine;

public class AggroRange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GolemKnightAI boss;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.isAggro = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.isAggro = false;
        }
    }
}
