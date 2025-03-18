using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRank", menuName = "Config/NewRank")]
public class RankBase : ScriptableObject, ISerializationCallbackReceiver
{
    [ReadOnlyInspector] public string KEY_ID; // Уникальный идентификатор ранга
    public Rank SourceRank;

    public Rank GetData()
    {
        return SourceRank.Clone();
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
            if(SourceRank != null) SourceRank.KEY_ID = KEY_ID; 
        }
    }

    public void SetLevelCurrent()
    { 
        int level = 0;

        if (SourceRank.PreviousRank != null)
        {
            level = SourceRank.PreviousRank.SourceRank.Rank_Level;
            level++;
        }

        SourceRank.Rank_Level = level;
    }
}

// Конструктор для инициализации ранга
[Serializable]
public class Rank
{
    public string KEY_ID;

    public Sprite Rank_Icon; // Иконка ранга
    public string Rank_Name; // Имя ранга

    public int Rank_Level;

    public RankBase NextRank;
    public RankBase PreviousRank;

    public int NeededExp;
    public int CurrentExp;
    public bool IsRewarded;
    public bool IsComplete;

    public Rank(Rank data)
    {
        KEY_ID = data.KEY_ID;
    }
    public Rank()
    {

    }

    public bool AddExp(int value)
    {
        CurrentExp += value;

        bool result = CurrentExp >= NeededExp;

        if (result) IsComplete = true;

        return result;
    }

    public Rank Clone()
    {
        return new Rank(this);
    }

    public Rank GetNextRankData()
    {
        return NextRank.SourceRank.Clone();
    }

    public Rank GetPreviousRankData()
    {
        return PreviousRank.SourceRank.Clone();
    }
}
