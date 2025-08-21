using TMPro;
using UnityEngine;

public class DebugMenuUI : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text everythingBarMaxText;
    public TMP_Text maxHealthText;
    public TMP_Text currentHealthText;

    private void Update()
    {
        if (gameManager != null)
        {
            everythingBarMaxText.text = $"{gameManager.everythingBarMax}";
            maxHealthText.text = $"{gameManager.playerMaxHealth}";
            currentHealthText.text = $"{gameManager.playerHealth}";
        }
    }
}
