using UnityEngine;

public class AggroArea2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CustomEnemyAI2 enemy;

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
            enemy.isAggro = true;
        }

    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            enemy.isAggro = false;
        }
        
    }
}
