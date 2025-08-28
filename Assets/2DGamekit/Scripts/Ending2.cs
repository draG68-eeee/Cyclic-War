using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Ending2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite[] cutscene;
    public Image image;
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
            image.sprite = cutscene[i];
            yield return new WaitForSeconds(3);
            i++;
        }
        
    }
}
