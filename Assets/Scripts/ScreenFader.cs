using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        // Singleton to persist across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start with a fade in
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1, 0); // from black to clear
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(0, 1); // from clear to black
    }

    IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = end;
    }

    // Helper to load scenes with fade
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndSwitchScene(sceneName));
    }

    IEnumerator FadeAndSwitchScene(string sceneName)
    {
        // Fade to black
        yield return FadeOut();

        // Load next scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Fade back in
        yield return FadeIn();
    }
}
