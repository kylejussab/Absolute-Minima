using System.Collections.Generic;
using UnityEngine;

public static class LevelSession
{
    // --- Run-level tracking ---
    public static LevelData CurrentLevel;
    public static int InkDrops = 0;
    public static List<LevelData> Levels = new List<LevelData>();
    public static HashSet<string> completedLevels = new HashSet<string>();

    // --- Player state (persists across scenes) ---
    public static List<Item> EquippedItems = new List<Item>();
    public static float EverythingBarMax = 100f;
    public static float PlayerMaxHealth = 100f;
    public static float PlayerHealth = 100f;
    public static bool inConversation = false;

    // --- Reset everything at the start of a new run ---
    public static void Reset()
    {
        CurrentLevel = null;
        InkDrops = 0;
        Levels.Clear();
        completedLevels.Clear();

        EquippedItems.Clear();
        EverythingBarMax = 100f;
        PlayerMaxHealth = 100f;
        PlayerHealth = 100f;

        // Starting items (only at beginning of run)
        AddItem(new Item { name = "Paintbrush", type = "Attack", value = 15 });
        AddItem(new Item { name = "Key", type = "Story", value = 10 });
    }

    // --- Item + health management ---
    public static void AddItem(Item item)
    {
        float projectedMax = PlayerMaxHealth - item.value;
        if (projectedMax >= 1f)
        {
            EquippedItems.Add(item);
            RecalculateHealth();
        }
    }

    public static void RemoveItem(Item item)
    {
        EquippedItems.Remove(item);
        RecalculateHealth();
    }

    public static float GetCurrentMaxHealth()
    {
        float totalReduction = 0f;
        foreach (var item in EquippedItems)
            totalReduction += Mathf.Max(0f, item.value);

        return Mathf.Max(0f, EverythingBarMax - totalReduction);
    }

    public static void RecalculateHealth()
    {
        float previousMax = PlayerMaxHealth;
        float previousHealth = PlayerHealth;

        PlayerMaxHealth = GetCurrentMaxHealth();
        float damageTaken = previousMax - previousHealth;
        PlayerHealth = Mathf.Max(PlayerMaxHealth - damageTaken, 0f);
    }
}
