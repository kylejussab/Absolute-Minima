using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private bool playerInside = false;
    private GameObject uiPrompt;
    private float yOffset = 1.5f;

    public void SetUIPrompt(GameObject prompt)
    {
        uiPrompt = prompt;
        uiPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInside)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * yOffset);
            uiPrompt.transform.position = screenPosition;

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerInside = false;
                LevelSession.completedLevels.Add(LevelSession.CurrentLevel.LevelNumber);
                LevelSession.InkDrops += LevelSession.CurrentLevel.Reward;
                ScreenFader.Instance.FadeToScene("Overview Map");
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
        if (other.CompareTag("Player") && playerInside)
        {
            playerInside = false;
            uiPrompt.SetActive(false);
        }
    }
}
