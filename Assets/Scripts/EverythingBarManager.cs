using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class BarSegment
{
    public string name;
    public float value = 0f;
    public RectTransform fill;
    public RectTransform icon;
}

public class EverythingBarManager : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private RectTransform container;

    [Header("Sizing")]
    [SerializeField] private float maxValue = 100f;
    [SerializeField] private bool useContainerWidth = true;
    [SerializeField] private float totalWidth = 420f;

    [Header("Health Segment (auto)")]
    [SerializeField] private RectTransform healthFill;
    [SerializeField] private RectTransform healthIcon;

    [Header("Icons")]
    [SerializeField] private float iconYOffset = -18f;

    [Header("Segment Prefab (optional)")]
    [SerializeField] private GameObject segmentPrefab;

    [SerializeField] private GameManager gm;

    private List<BarSegment> segments = new List<BarSegment>();

    private void Awake()
    {
        if (container == null) container = (RectTransform)transform;
        UpdateBars();
    }

    private void OnValidate()
    {
        if (container == null) container = (RectTransform)transform;
        UpdateBars();
    }

    private float GetTotalWidth()
    {
        if (useContainerWidth && container != null && container.rect.width > 0f)
            return container.rect.width;
        return totalWidth;
    }

    public void AddSegment(string name, float value, Sprite iconSprite = null)
    {
        if (segmentPrefab == null)
        {
            Debug.LogError("Segment prefab not assigned!");
            return;
        }

        GameObject newSegGO = Instantiate(segmentPrefab, container);

        RectTransform fill = newSegGO.GetComponent<RectTransform>();
        RectTransform icon = newSegGO.transform.Find("Image")?.GetComponent<RectTransform>();

        if (fill == null)
        {
            Debug.LogError("Segment prefab missing RectTransform on root!");
            Destroy(newSegGO);
            return;
        }

        BarSegment seg = new BarSegment
        {
            name = name,
            value = Mathf.Clamp(value, 0f, maxValue),
            fill = fill,
            icon = icon
        };

        if (icon != null && iconSprite != null)
        {
            Image img = icon.GetComponent<Image>();
            if (img != null)
                img.sprite = iconSprite;
        }

        seg.fill.anchoredPosition = new Vector2(0f, healthFill != null ? healthFill.anchoredPosition.y : 0f);
        if (seg.icon != null)
            seg.icon.anchoredPosition = new Vector2(0f, iconYOffset);

        segments.Add(seg);
        UpdateBars();
    }

    public void SetSegmentValue(int index, float value)
    {
        if (index < 0 || index >= segments.Count) return;
        segments[index].value = Mathf.Clamp(value, 0f, maxValue);
        UpdateBars();
    }

    public void ClearSegments()
    {
        foreach (var seg in segments)
        {
            if (seg.fill != null)
                Destroy(seg.fill.gameObject);
        }
        segments.Clear();
        UpdateBars();
    }

    private void UpdateBars()
    {
        float width = GetTotalWidth();

        // Total of all item segments
        float sumSegments = 0f;
        foreach (var seg in segments)
            sumSegments += seg.value;

        // Health is the leftover (max available)
        float maxAvailableHealth = Mathf.Max(0f, gm.everythingBarMax - sumSegments);

        // Clamp current health if it's more than max available
        if (gm.playerHealth > maxAvailableHealth)
            gm.playerHealth = maxAvailableHealth;

        float xOffset = 0f;

        // Health
        if (healthFill != null)
        {
            // Health width should reflect actual current health, not max
            float healthWidth = width * (gm.playerHealth / gm.everythingBarMax);
            healthFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthWidth);
            healthFill.anchoredPosition = new Vector2(xOffset, healthFill.anchoredPosition.y);

            // Icon stays fixed at start
            if (healthIcon != null)
                healthIcon.anchoredPosition = new Vector2(0f, iconYOffset);

            xOffset += width * (maxAvailableHealth / gm.everythingBarMax);
        }

        // Segments (attack, defense, story…)
        foreach (var seg in segments)
        {
            float segWidth = width * (seg.value / gm.everythingBarMax);

            if (seg.fill != null)
            {
                seg.fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, segWidth);
                seg.fill.anchoredPosition = new Vector2(
                    xOffset,
                    healthFill != null ? healthFill.anchoredPosition.y : seg.fill.anchoredPosition.y
                );
            }

            if (seg.icon != null)
                seg.icon.anchoredPosition = new Vector2(0f, iconYOffset);

            xOffset += segWidth;
        }
    }

    public void UpdateHealth(float currentHealth, float playerMaxHealth)
    {
        if (healthFill == null) return;

        float totalWidthAvailable = GetTotalWidth();

        float sumSegmentValues = 0f;
        foreach (var seg in segments)
            sumSegmentValues += seg.value;

        float healthAreaWidth = Mathf.Max(0f, totalWidthAvailable - (totalWidthAvailable * (sumSegmentValues / gm.everythingBarMax)));

        float currentHealthWidth = healthAreaWidth * Mathf.Clamp01(currentHealth / playerMaxHealth);

        healthFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHealthWidth);
        healthFill.anchoredPosition = new Vector2(0f, healthFill.anchoredPosition.y);
    }
}
