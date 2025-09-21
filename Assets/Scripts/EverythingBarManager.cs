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

    [SerializeField] private GameManager gameManager;

    private List<BarSegment> segments = new List<BarSegment>();

    private void Awake()
    {
        if (container == null) container = (RectTransform)transform;
        UpdateBars();
    }

    private void OnValidate()
    {
        if (container == null) container = (RectTransform)transform;
        if (Application.isPlaying)
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, mode) =>
            {
                UpdateBars();
            };
    }

    private float GetTotalWidth()
    {
        if (useContainerWidth && container != null && container.rect.width > 0f)
            return container.rect.width;
        return totalWidth;
    }

    public void AddSegment(string name, float value, Sprite iconSprite = null)
    {
        if (segmentPrefab == null) return;
        
        GameObject newSegmentObject = Instantiate(segmentPrefab, container);

        RectTransform fill = newSegmentObject.GetComponent<RectTransform>();
        RectTransform icon = newSegmentObject.transform.Find("Image")?.GetComponent<RectTransform>();

        if (fill == null)
        {
            Debug.LogError("Segment prefab missing RectTransform on root!");
            Destroy(newSegmentObject);
            return;
        }

        BarSegment segment = new BarSegment
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

        segment.fill.anchoredPosition = new Vector2(0f, healthFill != null ? healthFill.anchoredPosition.y : 0f);
        if (segment.icon != null)
            segment.icon.anchoredPosition = new Vector2(0f, iconYOffset);

        segments.Add(segment);
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
        foreach (var segment in segments)
        {
            if (segment.fill != null) Destroy(segment.fill.gameObject);
        }
        segments.Clear();
        UpdateBars();
    }

    private void UpdateBars()
    {
        float width = GetTotalWidth();

        float sumSegments = 0f;
        foreach (var segment in segments)
            sumSegments += segment.value;

        float maxAvailableHealth = Mathf.Max(0f, LevelSession.EverythingBarMax - sumSegments);

        if (LevelSession.PlayerHealth > maxAvailableHealth) LevelSession.PlayerHealth = maxAvailableHealth;

        float xOffset = 0f;

        if (healthFill != null)
        {
            float healthWidth = width * (LevelSession.PlayerHealth / LevelSession.EverythingBarMax);
            healthFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthWidth);
            healthFill.anchoredPosition = new Vector2(xOffset, healthFill.anchoredPosition.y);

            if (healthIcon != null)
                healthIcon.anchoredPosition = new Vector2(0f, iconYOffset);

            xOffset += width * (maxAvailableHealth / LevelSession.EverythingBarMax);
        }

        foreach (var segment in segments)
        {
            float segWidth = width * (segment.value / LevelSession.EverythingBarMax);

            if (segment.fill != null)
            {
                segment.fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, segWidth);
                segment.fill.anchoredPosition = new Vector2(
                    xOffset,
                    healthFill != null ? healthFill.anchoredPosition.y : segment.fill.anchoredPosition.y
                );
            }

            if (segment.icon != null)
                segment.icon.anchoredPosition = new Vector2(0f, iconYOffset);

            xOffset += segWidth;
        }
    }

    public void UpdateHealth(float currentHealth, float playerMaxHealth)
    {
        if (healthFill == null) return;

        float totalWidthAvailable = GetTotalWidth();

        float sumSegmentValues = 0f;
        foreach (var segment in segments)
            sumSegmentValues += segment.value;

        float healthAreaWidth = Mathf.Max(0f, totalWidthAvailable - (totalWidthAvailable * (sumSegmentValues / LevelSession.EverythingBarMax)));

        float currentHealthWidth = healthAreaWidth * Mathf.Clamp01(currentHealth / playerMaxHealth);

        healthFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHealthWidth);
        healthFill.anchoredPosition = new Vector2(0f, healthFill.anchoredPosition.y);
    }
}
