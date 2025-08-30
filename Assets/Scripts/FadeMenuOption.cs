using TMPro;
using UnityEngine;

public class FadeMenuOption : MonoBehaviour
{
    [Header("Fade Options")]
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] [Range(0f, 1f)] private float minAlpha = 0.15f;

    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float oscillation = (Mathf.Sin(Time.time * fadeSpeed) + 1f) / 2f;

        float alpha;
        if (oscillation < 0.5f)
            alpha = Mathf.Lerp(minAlpha, 0.6f, oscillation * 2f);
        else
        {
            float eased = Mathf.SmoothStep(0.6f, 1f, (oscillation - 0.5f) * 2f);
            alpha = eased;
        }

        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}
