using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Ending : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite[] cutscene;
    public Image cutsceneScreen;
    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PlayCutscene()
    {
        int i = 0;
        while (i < 7)
        {
            cutsceneScreen.sprite = cutscene[i];
            yield return new WaitForSeconds(5);
            i++;
        }
    }
}
