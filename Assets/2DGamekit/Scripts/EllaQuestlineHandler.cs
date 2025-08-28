using UnityEngine;
using System.Collections;
public class EllaQuestlineHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int QuestlinePhase = 4; // 1: inside cave. phase 2: waiting for first enemy group to be cleared. 3: arrive and wait for next enemy group to be cleared. 4: quest success, now second ending unlockable
    public bool canTalkWithPlayer;
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public Sprite[] dialogue1;
    public Sprite[] dialogue2;
    public Sprite[] dialogue3;
    public Sprite[] dialogue4;
    public MainDialogueManager dialogueManager;

    public GameObject domeGuardian1;

    public GameObject domeGuardian2;
    public GameObject domeGuardian3;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MainQuestlineHandler());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTalkWithPlayer)
        {
            Debug.Log("Interaction Successful");
            StartCoroutine(InteractWithPlayer());
        }
    }

    IEnumerator MainQuestlineHandler()
    {
        yield return new WaitUntil(() => QuestlinePhase == 2);
        transform.position = spawnPoint1.transform.position;
        yield return new WaitUntil(() => QuestlinePhase == 3);
        // walk to next position
        transform.position = spawnPoint2.transform.position;
        // rb.linearVelocity = Vector2.zero;
        // wait until next enemy camp is cleared
        yield return new WaitUntil(() => QuestlinePhase == 4);

    }

    IEnumerator InteractWithPlayer()
    {
        if (QuestlinePhase == 1)
        {
            yield return StartCoroutine(FirstMeeting());
            yield break;
        }

        if (QuestlinePhase == 2)
        {
            yield return StartCoroutine(EscortMissionPart1());
            yield break;
        }

        if (QuestlinePhase == 3)
        {
            yield return StartCoroutine(EscortMissionPart2());
            yield break;
        }

        if (QuestlinePhase == 4)
        {
            yield return StartCoroutine(QuestlineFinished());
            yield break;
        }


    }

    IEnumerator FirstMeeting()
    {
        // greetings, can you help us
        yield return dialogueManager.SetText(dialogue1);
        yield return new WaitForSeconds(1);
        QuestlinePhase = 2;
    }

    IEnumerator EscortMissionPart1()
    {
        // as much as i wish i could help, we are too weak to fight at the moment
        yield return dialogueManager.SetText(dialogue2);
        // insert wait until condition
        yield return new WaitUntil(() => domeGuardian1.active == false);
        yield return new WaitForSeconds(1);
        QuestlinePhase = 3;
    }

    IEnumerator EscortMissionPart2()
    {
        // we're almost at the end of this tundra. this is the last enemy camp
        yield return dialogueManager.SetText(dialogue3);
        // insert wait until condition
        yield return new WaitUntil(() => domeGuardian2.active == false);
        yield return new WaitUntil(() => domeGuardian3.active == false);
        yield return new WaitForSeconds(1);
        QuestlinePhase = 4;
        if (CheckpointManager.Instance != null) CheckpointManager.Instance.SetEllaQuestlinePhase(4);
    }

    IEnumerator QuestlineFinished()
    {
        // thank you for bringing us here. we should be able to make our way
        yield return dialogueManager.SetText(dialogue4);
        yield return new WaitForSeconds(1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalkWithPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalkWithPlayer = false;
        }
    }
}
