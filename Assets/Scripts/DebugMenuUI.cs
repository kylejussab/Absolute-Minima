using TMPro;
using UnityEngine;

public class DebugMenuUI : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text everythingBarMaxText;
    public TMP_Text maxHealthText;
    public TMP_Text currentHealthText;

    public DungeonGenerator dungeonGenerator;
    public TMP_Text roomsText;
    public TMP_Text shopText;
    public TMP_Text rewardText;

    private void Update()
    {
        if (gameManager != null && dungeonGenerator != null)
        {
            everythingBarMaxText.text = $"{gameManager.everythingBarMax}";
            maxHealthText.text = $"{gameManager.playerMaxHealth}";
            currentHealthText.text = $"{gameManager.playerHealth}";

            roomsText.text = $"{dungeonGenerator.backboneLength}";
            shopText.text = $"{Mathf.Round(dungeonGenerator.branchChance * 100)}";
            rewardText.text = $"{dungeonGenerator.levelReward}";
        }
    }
}
