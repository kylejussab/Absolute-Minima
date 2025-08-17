using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnderlineMenuOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text tmpText;
    private string originalText;

    void Awake()
    {
        // Look in children if TMP_Text is not on the same object
        tmpText = GetComponentInChildren<TMP_Text>();
        if (tmpText != null)
            originalText = tmpText.text;
        else
            Debug.LogError("TMP_Text not found in children of " + gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmpText.text = "<u>" + originalText + "</u>"; // Add underline
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmpText.text = originalText; // Remove underline
    }
}
