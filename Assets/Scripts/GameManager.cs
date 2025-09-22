using System.Collections.Generic;
using UnityEngine;

// Per-level manager that hooks the static state into the scene
public class GameManager : MonoBehaviour
{
    public EverythingBarManager barManager;

    [Header("Segment Icons")]
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite defenseIcon;
    [SerializeField] private Sprite storyIcon;

    private void Start()
    {
        // Sync LevelSession state into the UI
        UpdateUI();
        barManager.UpdateHealth(LevelSession.PlayerHealth, LevelSession.PlayerMaxHealth);
    }

    private void UpdateUI()
    {
        barManager.ClearSegments();

        foreach (var item in LevelSession.EquippedItems)
        {
            // The items in LevelSession don’t hold icons anymore
            Sprite icon = item.type switch
            {
                "Attack" => attackIcon,
                "Defense" => defenseIcon,
                "Story" => storyIcon,
                _ => null
            };

            barManager.AddSegment(item.type, item.value, icon);
        }
    }

    // Debug keys for testing
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            LevelSession.AddItem(new Item { name = "Debug Sword", type = "Attack", value = 10 });

        if (Input.GetKeyDown(KeyCode.Alpha2))
            LevelSession.AddItem(new Item { name = "Debug Shield", type = "Defense", value = 15 });

        if (Input.GetKeyDown(KeyCode.Alpha0) && LevelSession.EquippedItems.Count > 0)
            LevelSession.RemoveItem(LevelSession.EquippedItems[^1]);

        if (Input.GetKeyDown(KeyCode.H))
        {
            LevelSession.PlayerHealth = Mathf.Max(0f, LevelSession.PlayerHealth - 10f);
        }

        UpdateUI();
        barManager.UpdateHealth(LevelSession.PlayerHealth, LevelSession.PlayerMaxHealth);
    }
}
