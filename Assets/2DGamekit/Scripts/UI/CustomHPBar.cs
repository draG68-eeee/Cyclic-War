using UnityEngine;
using Gamekit2D;
public class CustomHPBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RectTransform rectTransform;
    public Damageable damageable;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();
    }


    private void UpdateBar()
    {
        int current = damageable.CurrentHealth;
        float currentFill = (float)current / 5;
        rectTransform.localScale = new Vector3(currentFill, 1, 1);
        // yield return new WaitForSeconds(2);
        
    }

}
