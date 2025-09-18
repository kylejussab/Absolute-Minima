using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EverythingBarManager barManager;

    public List<Item> equippedItems = new List<Item>();

    [Header("Segment Icons")]
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite defenseIcon;
    [SerializeField] private Sprite storyIcon;

    public float everythingBarMax = 100f;
    public float playerMaxHealth = 0f;
    public float playerHealth = 100f;

    private void Start()
    {
        // Example: populate starting items
        AddItem(new Item { name = "Paintbrush", type = "Attack", value = 15, icon = attackIcon });
        AddItem(new Item { name = "Key", type = "Story", value = 10, icon = storyIcon });

        playerMaxHealth = GetCurrentMaxHealth();
        playerHealth = playerMaxHealth;
    }

    public void AddItem(Item item)
    {
        float projectedMax = playerMaxHealth - item.value;
        if (projectedMax >= 1f)
        {
            equippedItems.Add(item);
            RecalculateHealth();
            UpdateUI();
        }
    }

    public void RemoveItem(Item item)
    {
        equippedItems.Remove(item);
        RecalculateHealth();
        UpdateUI();
    }

    private void UpdateUI()
    {
        barManager.ClearSegments();

        foreach (var item in equippedItems)
        {
            barManager.AddSegment(item.type, item.value, item.icon);
        }
    }

    public float GetCurrentMaxHealth()
    {
        float totalReduction = 0f;

        foreach (var item in equippedItems)
        {
            totalReduction += Mathf.Max(0f, item.value);
        }

        return Mathf.Max(0f, everythingBarMax - totalReduction);
    }

    public void RecalculateHealth()
    {
        float previousMax = playerMaxHealth;
        float previousHealth = playerHealth;

        playerMaxHealth = GetCurrentMaxHealth();

        float damageTaken = previousMax - previousHealth;

        playerHealth = Mathf.Max(playerMaxHealth - damageTaken, 0f);

        if (barManager != null)
            barManager.UpdateHealth(playerHealth, playerMaxHealth);
    }

    // DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddItem(new Item { name = "Debug Sword", type = "Attack", value = 10, icon = attackIcon });

        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddItem(new Item { name = "Debug Shield", type = "Defense", value = 15, icon = defenseIcon });

        if (Input.GetKeyDown(KeyCode.Alpha0) && equippedItems.Count > 0)
            RemoveItem(equippedItems[equippedItems.Count - 1]);

        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHealth -= 10f;
            playerHealth = Mathf.Max(0f, playerHealth);
            barManager.UpdateHealth(playerHealth, playerMaxHealth);
        }
    }
}
