using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipConfig", menuName = "Config/Equip")]
public class EquipConfig : Config
{
    public List<EquipBase> DataBase;

    private Dictionary<string, EquipBase> _equipData;


    public override IEnumerator Init()
    {
        _equipData = new Dictionary<string, EquipBase>();

        foreach (var equip in DataBase)
        {
            _equipData.Add(equip.KEY_ID, equip);
        }

        yield return new WaitForSeconds(0.1f);
    }

    public EquipBase GetEquipBase(string id)
    {
        return _equipData[id];
    }

    public Equip LoadEquip(ItemData data)
    {
        if (data == null) return null;
        if (string.IsNullOrEmpty(data.KEY_ID)) return null;

        if (_equipData.TryGetValue(data.KEY_ID, out EquipBase value))
        {
            var loadedEquip = value.GetEquip();

            loadedEquip.LoadData(data);

            return loadedEquip;
        }

        Debug.LogError($"Item with {data.KEY_ID} ID doesnt exist");

        return null;
    }
    public T LoadEquip<T>(ItemData data) where T : Equip
    {
        if (data == null) return null;
        if (string.IsNullOrEmpty(data.KEY_ID)) return null;

        if (_equipData.TryGetValue(data.KEY_ID, out EquipBase value))
        {
            var loadedEquip = value.GetEquip();

            loadedEquip.LoadData(data);

            return loadedEquip as T;
        }

        Debug.LogError($"Item with {data.KEY_ID} ID doesnt exist");

        return null;
    }
}
