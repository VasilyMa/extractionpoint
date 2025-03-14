using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCase", menuName = "Config/NewCase")]
public class CaseBase : ScriptableObject, ISerializationCallbackReceiver
{
    [ReadOnlyInspector] public string KEY_ID;
    public Case SourceCase;

    public Case GetData()
    {
        return SourceCase.Clone();
    }

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
        if (!string.IsNullOrEmpty(name))
        {
            KEY_ID = name;
            if (SourceCase != null)
            {
                SourceCase.KEY_ID = KEY_ID;
            }
        }
    }
}

[Serializable]
public class Case
{
    public string KEY_ID;                     // Уникальный идентификатор кейса
    public string CaseName;                   // Название кейса
    public Sprite CaseIcon;                   // Иконка кейса
    public GameObject CasePrefab;                   // Иконка кейса
    public List<EquipBase> possibleEquip;     // Возможные предметы в кейсе
    public int CasePrice;                     // Цена кейса в валюте
    public float DropRate;                    // Шанс выпадения кейса

    public Case(Case data)
    {
        if (data != null)
        {
            KEY_ID = data.KEY_ID;
            CaseName = data.CaseName;
            CaseIcon = data.CaseIcon;
            possibleEquip = new List<EquipBase>(data.possibleEquip);
            CasePrice = data.CasePrice;
            DropRate = data.DropRate;
        }
    }
    public Case Clone()
    {
        return new Case(this);
    }
}
