using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class MainDialogueManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image imageDialogue;
    public GameObject dialogueBox;
    public GameObject dialogueBox2;
    private bool canBeUsed = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator SetText(Sprite[] dialogueSpeeches)
    {
        Debug.Log("recieved");
        if (canBeUsed)
        {
            canBeUsed = false;
            dialogueBox.SetActive(true);
            dialogueBox2.SetActive(true);
            imageDialogue.gameObject.SetActive(true);
            yield return FlipThroughText(dialogueSpeeches);
            canBeUsed = true;
            dialogueBox.SetActive(false);
            imageDialogue.gameObject.SetActive(false);
            dialogueBox2.SetActive(false);
        }
    }

    IEnumerator FlipThroughText(Sprite[] dialogue)
    {
        int i = 0;
        while (i < dialogue.Length)
        {
            imageDialogue.sprite = dialogue[i];
            yield return new WaitForSeconds(5);
            i++;
        }
    }
}
