using TMPro;
using UnityEngine;

public class DebugMenuUI : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text maxHealthText;
    public TMP_Text currentHealthText;

    private void Update()
    {
        if (gameManager != null)
        {
            maxHealthText.text = $"{gameManager.playerMaxHealth}";
            currentHealthText.text = $"{gameManager.playerCurrentHealth}";
        }
    }
}
