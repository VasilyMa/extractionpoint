using UnityEngine;

public abstract class EquipBase : ScriptableObject, ISerializationCallbackReceiver
{
    [ReadOnlyInspector] public string KEY_ID;

    public abstract Equip GenerateEquip();
    public abstract Equip GetEquip();
    public abstract T GetEquip<T>() where T : Equip;

    public virtual void OnAfterDeserialize()
    {

    }

    public virtual void OnBeforeSerialize()
    {
        if (this != null)
        {
            KEY_ID = name;
        }
    }
}
