using System.Linq;
using TMPro;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private bool playerInside = false;
    private GameObject uiPrompt;
    private TextMeshProUGUI promptText;
    private float yOffset = 1f;

    private bool hasKey;

    public void SetUIPrompt(GameObject prompt)
    {
        uiPrompt = prompt;
        promptText = uiPrompt.GetComponentInChildren<TextMeshProUGUI>();
        uiPrompt.SetActive(false);
    }

    void Update()
    {
        hasKey = LevelSession.EquippedItems.Any(item => item.name == "Key");

        if (playerInside)
        {
            promptText.text = hasKey ? "Press E to use key" : "Key Required";

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * yOffset);
            uiPrompt.transform.position = screenPosition;

            if (Input.GetKeyDown(KeyCode.E) && hasKey)
            {
                uiPrompt.SetActive(false);
                var keyItem = LevelSession.EquippedItems.FirstOrDefault(item => item.name == "Key");
                LevelSession.RemoveItem(keyItem);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            uiPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            uiPrompt.SetActive(false);
        }
    }
}
