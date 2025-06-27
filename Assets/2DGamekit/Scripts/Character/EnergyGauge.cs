using UnityEngine;
using UnityEngine.UI; // ✅ UGUI, not UIElements
using System.Collections;
using Gamekit2D;
public class EnergyGauge : MonoBehaviour
{
    [Range(0, 100)]
    public int energy = 0;

    public Sprite fullGauge;       // Sprite to show when full
    public Image image;            // UI fill image
    public Damager damager;
    public Sprite defaultGauge;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (image == null)
        {
            // image = GetComponent<Image>(); // fallback if not assigned
        }

        UpdateGauge();
    }

    void Update()
    {
        UpdateGauge(); // Optional: could remove if using only UpdateEnergy()
        UpdateDamage();
        // yield return ResetGauge();
    }

    private Coroutine resetCoroutine = null;

    void UpdateGauge()
    {
        float scaleX = Mathf.Clamp01((float)energy / 100f);
        rectTransform.localScale = new Vector3(scaleX, 1, 1);

        if (energy == 100 && fullGauge != null)
        {
            image.sprite = fullGauge;
            if (resetCoroutine == null)
            {
                resetCoroutine = StartCoroutine(ResetGauge());
            }
        }
        else
        {
            image.sprite = defaultGauge;
            // If gauge is not full, cancel any running reset coroutine
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
                resetCoroutine = null;
            }
        }
    }
    IEnumerator ResetGauge()
    {
        // Only reset if still full after the wait
        yield return new WaitForSeconds(10);
        if (energy == 100)
        {
            energy = 0;
            UpdateGauge();
        }
        resetCoroutine = null;
    }

    public void UpdateEnergy(int value)
    {
        energy += value;
        energy = Mathf.Clamp(energy, 0, 100);
        UpdateGauge(); // update visuals after change
    }

    public float GetDamageMultiplier()
    {
        return Mathf.Lerp(1f, 2f, energy / 100); // scale from 1x to 2x damage
    }

    public void UpdateDamage()
    {
        damager.damage = (int)GetDamageMultiplier();
    }
}
