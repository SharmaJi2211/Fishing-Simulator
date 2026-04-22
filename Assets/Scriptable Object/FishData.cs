using UnityEngine;

[CreateAssetMenu(menuName = "Fishing/FishData", fileName = "NewFish")]
public class FishData : ScriptableObject
{
    [Header("Identity")]
    public string fishName;
    public Sprite icon;

    [Header("Rarity")]
    public Rarity rarity;
    public int spawnWeight; // controls how likely each fish is to appear when you cast

    [Header("Minigame")]
    public float zoneWidth        = 0.25f;
    public float fishSpeed        = 1.2f;
    public float struggleInterval = 1.8f;
    public float catchFillRate    = 0.12f;
    public float catchDrainRate   = 0.09f;
}

public enum Rarity { Common, Uncommon, Rare }