using UnityEngine;
using System.Collections;
using Gamekit2D;
public class UpdateHPBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RectTransform healthBar;
    public RectTransform healthFrame;
    public Damageable damageable;
    public GameObject panel;
    private float newValue;
    void Start()
    {
        Disable();
    }

    // Update is called once per frame
    void Update()
    {
        newValue = (float)damageable.CurrentHealth / 50;
        healthBar.localScale = new Vector3(newValue, 1, 1);
        if (newValue == 0)
        {
            Disable();
        }
        // UpdateDelta();
    }

    public void Enable()
    {
        panel.SetActive(true);
    }

    public void Disable()
    {
        panel.SetActive(false);
    }


}
