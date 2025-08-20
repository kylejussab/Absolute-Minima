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

    public float playerMaxHealth = 100f;
    public float playerCurrentHealth = 0f;

    private void Start()
    {
        // Example: populate starting items
        AddItem(new Item { name = "Paintbrush", type = "Attack", value = 15, icon = attackIcon });
        AddItem(new Item { name = "Key", type = "Story", value = 10, icon = storyIcon });
        playerCurrentHealth = GetCurrentHealth();
    }

    public void AddItem(Item item)
    {
        equippedItems.Add(item);
        playerCurrentHealth = GetCurrentHealth();
        UpdateUI();
    }

    public void RemoveItem(Item item)
    {
        equippedItems.Remove(item);
        playerCurrentHealth = GetCurrentHealth();
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Clear old segments (you could add a ClearSegments() function in EverythingBarManager)
        barManager.ClearSegments();

        // Re-add segments based on equipped items
        foreach (var item in equippedItems)
        {
            barManager.AddSegment(item.type, item.value, item.icon);
        }
    }

    public float GetCurrentHealth()
    {
        float totalReduction = 0f;
        foreach (var item in equippedItems)
        {
            totalReduction += item.value;
        }

        return Mathf.Max(0f, playerMaxHealth - totalReduction);
    }

    // DEBUG
    private void Update()
    {
        // Add items with keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddItem(new Item { name = "Debug Sword", type = "Attack", value = 10, icon = attackIcon });

        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddItem(new Item { name = "Debug Shield", type = "Defense", value = 15, icon = defenseIcon });

        // Remove last item
        if (Input.GetKeyDown(KeyCode.Alpha0) && equippedItems.Count > 0)
            RemoveItem(equippedItems[equippedItems.Count - 1]);
    }
}
