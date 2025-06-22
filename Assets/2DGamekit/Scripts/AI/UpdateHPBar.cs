using UnityEngine;
using System.Collections;
using Gamekit2D;
public class UpdateHPBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RectTransform healthBar;
    public RectTransform healthFrame;
    public Damageable damageable;
    private float newValue;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        newValue = (float)damageable.CurrentHealth / 50;
        healthBar.localScale = new Vector3(newValue, 1, 1);
        UpdateDelta();
    }

    IEnumerator UpdateDelta()
    {
        Debug.Log("Her");
        yield return new WaitForSeconds(1);
        healthFrame.localScale = new Vector3(newValue,1,1);
    }
}
