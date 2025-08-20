using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class BarSegment
{
    public string name;                      // e.g., "Defense", "Attack", "Story"
    public float value = 0f;                 // The segment value
    public RectTransform fill;               // The UI fill rectangle
    public RectTransform icon;               // Icon above the bar
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
    [SerializeField] private GameObject segmentPrefab; // prefab containing fill & icon

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

    /// <summary>
    /// Add a new segment dynamically from a prefab
    /// </summary>
    public void AddSegment(string name, float value, Sprite iconSprite = null)
    {
        if (segmentPrefab == null)
        {
            Debug.LogError("Segment prefab not assigned!");
            return;
        }

        GameObject newSegGO = Instantiate(segmentPrefab, container);

        // Prefab structure: root = fill, child = icon Image
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

        // Set icon sprite if provided
        if (icon != null && iconSprite != null)
        {
            Image img = icon.GetComponent<Image>();
            if (img != null)
                img.sprite = iconSprite;
        }

        // Reset positions
        seg.fill.anchoredPosition = new Vector2(0f, healthFill != null ? healthFill.anchoredPosition.y : 0f);
        if (seg.icon != null)
            seg.icon.anchoredPosition = new Vector2(0f, iconYOffset);

        segments.Add(seg);
        UpdateBars();
    }

    /// <summary>
    /// Update value of an existing segment
    /// </summary>
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

        // Sum of all segments
        float sumSegments = 0f;
        foreach (var seg in segments)
            sumSegments += seg.value;

        // Health is remainder
        float healthValue = Mathf.Max(0f, maxValue - sumSegments);

        float xOffset = 0f;

        // Health
        if (healthFill != null)
        {
            float healthWidth = width * (healthValue / maxValue);
            healthFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthWidth);
            healthFill.anchoredPosition = new Vector2(xOffset, healthFill.anchoredPosition.y);

            if (healthIcon != null)
                healthIcon.anchoredPosition = new Vector2(0f, iconYOffset); // local 0 stays centered

            xOffset += healthWidth;
        }

        // Other segments
        foreach (var seg in segments)
        {
            float segWidth = width * (seg.value / maxValue);

            if (seg.fill != null)
            {
                seg.fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, segWidth);
                seg.fill.anchoredPosition = new Vector2(xOffset, healthFill != null ? healthFill.anchoredPosition.y : seg.fill.anchoredPosition.y);
            }

            if (seg.icon != null)
                seg.icon.anchoredPosition = new Vector2(0f, iconYOffset); // local 0 stays centered

            xOffset += segWidth;
        }
    }
}
