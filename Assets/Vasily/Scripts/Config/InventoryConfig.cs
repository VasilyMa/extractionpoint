using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Config/Inventory")]
public class InventoryConfig : Config
{
    public int MaxSizeInventory;

    [ReadOnlyInspector]public const string SlotName = "SLOT_";

    public override IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
