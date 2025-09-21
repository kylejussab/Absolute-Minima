using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            LevelSession.completedLevels.Add(LevelSession.CurrentLevel.LevelNumber);
            LevelSession.InkDrops += LevelSession.CurrentLevel.Reward;
            Debug.Log(LevelSession.InkDrops);
            ScreenFader.Instance.FadeToScene("Overview Map");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
