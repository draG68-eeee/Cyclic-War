using UnityEngine;
using UnityEngine.SceneManagement;
public class BringPlayerToEnding : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Here");
        if (other.CompareTag("Player"))
        {

            SceneManager.LoadScene("Ending 1 scene");
        }
    }
}
