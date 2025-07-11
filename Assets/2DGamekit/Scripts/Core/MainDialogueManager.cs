using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class MainDialogueManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image imageDialogue;
    private bool canBeUsed = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SetText(Sprite[] dialogueSpeeches)
    {
        if (canBeUsed)
        {
            canBeUsed = false;
            yield return FlipThroughText(dialogueSpeeches);
            canBeUsed = true;
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
