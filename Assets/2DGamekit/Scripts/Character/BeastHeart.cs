using UnityEngine;
using System.Collections;
using Gamekit2D;
public class BeastHeart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Damageable playerDamageable;
    private bool canUse = true;
    public float cooldownTime = 9;
    public RectTransform rectTransform;
    private float scale;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canUse)
        {
            StartCoroutine(UpdateScale());
            Debug.Log("Tried Healing");
            StartCoroutine(ResetAbility());
            playerDamageable.GainHealth(3);

        }
    }

    IEnumerator ResetAbility()
    {
        canUse = false;
        yield return new WaitForSeconds(cooldownTime);
        canUse = true;
    }

    IEnumerator UpdateScale()
    {
        scale = 0;
        // int i = 0;
        rectTransform.localScale = new Vector3(scale / 9, 1, 1);
        while (scale < 9)
        {
            yield return new WaitForSeconds(1);
            scale++;
            rectTransform.localScale = new Vector3(scale / 9, 1, 1);
            // i++;
        }
    }
    

}
