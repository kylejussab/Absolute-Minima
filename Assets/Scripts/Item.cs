using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;   // "Paintbrush"
    public string type;       // "Attack", "Defense", "Story", etc.
    public float value;       // e.g., 10
    public Sprite icon;       // Icon to show on the UI
}
