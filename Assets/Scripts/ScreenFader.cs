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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1, 0);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(0, 1);
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

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndSwitchScene(sceneName));
    }

    IEnumerator FadeAndSwitchScene(string sceneName)
    {
        yield return FadeOut();
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return FadeIn();
    }
}
