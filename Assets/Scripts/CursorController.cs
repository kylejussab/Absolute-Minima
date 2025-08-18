using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CursorController : MonoBehaviour
{
    [Header("Emote Times")]
    public float emoteDelay = 0.15f;
    public float emoteDuration = 0.5f;

    [Header("Cursor Sprites")]
    [SerializeField] private Image cursorImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite clickSprite;
    [SerializeField] private Sprite[] positiveEmotes;
    [SerializeField] private Sprite negativeEmote;

    private bool isAnimating = false;

    void Awake()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            cursorImage.canvas.transform as RectTransform,
            Input.mousePosition,
            cursorImage.canvas.worldCamera,
            out pos
        );
        cursorImage.rectTransform.localPosition = pos;

        if (Input.GetMouseButton(0) && !isAnimating)
        {
            cursorImage.sprite = clickSprite;
            isAnimating = false;
        }
        else if (Input.GetMouseButtonUp(0))
            if (!isAnimating)
                StartCoroutine(EmoteSequence());
        else if (!isAnimating)
            cursorImage.sprite = defaultSprite;
    }

    IEnumerator EmoteSequence()
    {
        isAnimating = true;

        bool wasOverUI = EventSystem.current.IsPointerOverGameObject();

        yield return new WaitForSeconds(emoteDelay);

        if (wasOverUI && positiveEmotes.Length > 0)
        {
            int index = Random.Range(0, positiveEmotes.Length);
            cursorImage.sprite = positiveEmotes[index];
        }
        else
            cursorImage.sprite = negativeEmote;

        yield return new WaitForSeconds(emoteDuration);

        cursorImage.sprite = defaultSprite;
        isAnimating = false;
    }
}
