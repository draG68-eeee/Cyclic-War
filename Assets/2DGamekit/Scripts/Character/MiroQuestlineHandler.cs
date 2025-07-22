using UnityEngine;
using System.Collections;
public class MiroQuestlineHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MainDialogueManager dialogueManager;
    public int questlinePhase = 1;
    public GameObject player;
    public Sprite[] dialogue1;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private bool playerInRange = false;
    private bool dialogueActive = false;

    void Update()
    {
        if (playerInRange && !dialogueActive && Gamekit2D.PlayerInput.Instance.Interact.Down)
        {
            StartCoroutine(StartQuestlineDialogue());
            Debug.Log("here");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    private IEnumerator StartQuestlineDialogue()
    {
        dialogueActive = true;
        if (questlinePhase == 1 && dialogue1 != null && dialogue1.Length > 0)
        {
            yield return StartCoroutine(dialogueManager.SetText(dialogue1));
        }
        // Add further questline phases as needed
        dialogueActive = false;
    }
}

