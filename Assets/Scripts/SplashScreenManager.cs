using System.Collections;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    [Header("Settings")]
    public float transitionTime = 3.0f;
    public float fadeDuration = 0.5f;

    [Header("Screen Panels")]
    [SerializeField] private CanvasGroup splashKyle;
    [SerializeField] private CanvasGroup splashUnity;
    [SerializeField] private CanvasGroup splashTitle;

    void Start()
    {
        StartCoroutine(PlaySplashSequence());
    }

    IEnumerator PlaySplashSequence()
    {
        // Panel 1
        yield return StartCoroutine(FadePanel(splashKyle, true));
        yield return new WaitForSeconds(transitionTime);
        yield return StartCoroutine(FadePanel(splashKyle, false));

        // Panel 2
        yield return StartCoroutine(FadePanel(splashUnity, true));
        yield return new WaitForSeconds(transitionTime);
        yield return StartCoroutine(FadePanel(splashUnity, false));

        // Panel 3
        yield return StartCoroutine(FadePanel(splashTitle, true));

        fadeDuration = 0.1f;
        while (!Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        while (Input.GetMouseButton(0)) // prevent hold-click
        {
            yield return null;
        }

        // Fade out title
        yield return StartCoroutine(FadePanel(splashTitle, false));

        // Load main menu scene
        ScreenFader.Instance.FadeToScene("Main Menu");
    }

    IEnumerator FadePanel(CanvasGroup canvasGroup, bool fadeIn)
    {
        float start = fadeIn ? 0f : 1f;
        float end = fadeIn ? 1f : 0f;
        float elapsed = 0f;

        if (fadeIn)
            canvasGroup.gameObject.SetActive(true);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = end;

        if (!fadeIn)
            canvasGroup.gameObject.SetActive(false);
    }
}
