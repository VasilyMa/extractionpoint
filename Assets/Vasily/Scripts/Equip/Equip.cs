
using UnityEngine;

[System.Serializable]
public abstract class Equip
{
    [ReadOnlyInspector] public string ID;
    [Space(10f)]
    [Header("Info")]
    public string Description;
    public string MaintenceName;
    public Rarity Rarity;
    public Sprite Icon;

    public abstract Equip Clone();
    public abstract ItemData GetData();
    public abstract void LoadData(ItemData data);
}

public enum Rarity { common, uncommon, rare, legendary }