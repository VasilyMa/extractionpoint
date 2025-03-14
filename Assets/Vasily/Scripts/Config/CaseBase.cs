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
    public string KEY_ID;                     // ���������� ������������� �����
    public string CaseName;                   // �������� �����
    public Sprite CaseIcon;                   // ������ �����
    public GameObject CasePrefab;                   // ������ �����
    public List<EquipBase> possibleEquip;     // ��������� �������� � �����
    public int CasePrice;                     // ���� ����� � ������
    public float DropRate;                    // ���� ��������� �����

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
